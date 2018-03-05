using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Input = UnityEngine.Input;

namespace CommandConsole
{
    public class ConsoleUi : MonoBehaviour
    {
        [SerializeField] private bool _useEssentialCommands;
        [SerializeField] private KeyCode _openCloseConsoleKey = KeyCode.F6;
        [SerializeField] private KeyCode _completeCommandKeyCode = KeyCode.Tab;
        [SerializeField] private KeyCode _historyNextKeyCode = KeyCode.UpArrow;
        [SerializeField] private KeyCode _historyPreviousKeyCode = KeyCode.DownArrow;
        [SerializeField] private GameObject _consolePanel;
        [SerializeField] private InputField _inputField;
        [SerializeField] private Text _textField;
        [SerializeField] private RectTransform _contentField;
        [SerializeField] private ScrollRect _scrollRect;

        private RectTransform _panelRectTransform;
        private float _startHeight;
        private bool _isActive;
        private float _panelHeight;
        private Vector3 _activePos;
        private Vector3 _inActivePos;
        private EssentialCommands _essentialCommands;

        private void Start()
        {
            //Register the Help command
            Console.Instance.RegisterCommand(new ConsoleCommand("help", arguments => Console.Instance.Help(), returnMessage: null));
            //Register the cls command to clear the console
            Console.Instance.RegisterCommand(new ConsoleCommand("cls", arguments => Clear(), returnMessage: null));

            if (_useEssentialCommands)
            {
                _essentialCommands = new EssentialCommands();
                Console.Instance.RegisterClass<EssentialCommands>(_essentialCommands);
            }

            _startHeight = _contentField.rect.height;
            Console.Instance.OnLog += Log;

            _isActive = false;
            _panelRectTransform = _consolePanel.GetComponent<RectTransform>();
            _activePos = _panelRectTransform.position;
            _panelHeight = _panelRectTransform.rect.height;
            _inActivePos = new Vector3(_panelRectTransform.position.x, _panelRectTransform.position.y + _panelHeight, _panelRectTransform.position.z);
            _panelRectTransform.position = _inActivePos;
            _consolePanel.SetActive(false);
        }

        private void OnDestroy()
        {
            Console.Instance.DeregisterCommand("help");
            Console.Instance.DeregisterCommand("cls");
            Console.Instance.DeregisterClass<EssentialCommands>(_essentialCommands);
            Console.Instance.OnLog -= Log;
        }

        private void Update()
        {
            if (_isActive)
            {
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    Console.Instance.HistoryManager.AddToHistory(_inputField.text);
                    Console.Instance.Execute(_inputField.text);
                    _inputField.text = "";
                }

                if (Input.GetKeyDown(_historyNextKeyCode))
                {
                    _inputField.text = Console.Instance.HistoryManager.Up();
                    _inputField.MoveTextEnd(false);
                }

                if (Input.GetKeyDown(_historyPreviousKeyCode))
                {
                    _inputField.text = Console.Instance.HistoryManager.Down();
                    _inputField.MoveTextEnd(false);
                }

                if (Input.GetKeyDown(_openCloseConsoleKey))
                {
                    StartCoroutine(Deactivate());
                }

                if (Input.GetKeyDown(_completeCommandKeyCode))
                {
                    var matchingCommands = Console.Instance.GetMatchingCommands(_inputField.text);
                    if (matchingCommands.Count == 1)
                    {
                        _inputField.text = matchingCommands.First();
                        _inputField.MoveTextEnd(false);
                    }
                    else
                    {
                        foreach (var cmd in matchingCommands)
                        {
                            Log(cmd);
                        }
                        if(matchingCommands.Count != 0) Log(" ");
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(_openCloseConsoleKey))
                {
                    StartCoroutine(Acitvate());
                }
            }
        }

        private IEnumerator Acitvate()
        {
            if (!_isActive)
            {
                _isActive = true;
                Console.Instance.IsAcitve = true;
                _consolePanel.SetActive(true);
                //Animate Panel appearance
                yield return StartCoroutine(Move(_panelRectTransform, _activePos, 0.5f));
                EventSystem.current.SetSelectedGameObject(_inputField.gameObject, null);
            }
        }

        private IEnumerator Deactivate()
        {
            if (_isActive)
            {
                _isActive = false;
                Console.Instance.IsAcitve = false;
                //Animate Panel appearance
                yield return StartCoroutine(Move(_panelRectTransform, _inActivePos, 0.5f));
                _consolePanel.SetActive(false);
            }
        }

        private IEnumerator Move(Transform transform, Vector3 to, float travelTime)
        {
            var startTime = Time.time;
            var startPos = transform.position;

            var t = 0f;
            while (t < 1f)
            {
                t = (Time.time - startTime) / travelTime;
                transform.position = Vector3.Lerp(startPos, to, t);
                yield return new WaitForEndOfFrame();
            }
        }

        private void Log(object sender, Console.ConsoleLogEventArgs args)
        {
            Log(args.Message);
        }

        private void Log(string message)
        {
            _textField.text += message + "\n";
            if (_textField.preferredHeight > _startHeight)
            {
                //Extend the scroll content
                _contentField.sizeDelta = new Vector2(_contentField.sizeDelta.x, _textField.preferredHeight);
                //Scroll to bottom
                _scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        private void OnGUI()
        {
            if(!_isActive) return;

            //Use the Keys to prevent that unity switches the active UI element on tab or enter.
            if (Event.current.keyCode == _completeCommandKeyCode && Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
            {
                Event.current.Use();
            }

            if (Event.current.keyCode == KeyCode.Return && Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
            {
                Event.current.Use();
            }
        }

        private void Clear()
        {
            _textField.text = "";
            _contentField.sizeDelta = new Vector2(_contentField.sizeDelta.x, _startHeight);
        }
    }
}