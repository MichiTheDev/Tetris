using JetBrains.Annotations;
using TwodeUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris
{
    public sealed class LevelManager : Singleton<LevelManager>
    {
        [Header("Board Settings")]
        [SerializeField] private int _boardWidth = 10;
        [SerializeField] private int _boardHeight = 18;

        [Header("Game Settings")]
        [SerializeField] private int _tickRate = 4;
        
        [NotNull] private Board _board = null!;
        [NotNull] private TetrisInput _input = null!;
        private float _tickRateTimer;
        private float _tickRateTime;

        protected override void Awake()
        {
            base.Awake();
            _board = new Board(_boardWidth, _boardHeight);
            _tickRateTime = 1f / _tickRate;
            _input = new TetrisInput();
        }

        private void OnEnable()
        {
            _input.Player.Movement!.started += MovementInput;
            _input.Player.Drop!.started += DropInput;
            _input.Player.SlowDrop!.performed += SlowDropInput;
            _input.Player.SlowDrop!.canceled += SlowDropInput;
            _input.Player.Rotate!.started += RotateInput;
            
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Movement!.started -= MovementInput;
            _input.Player.Drop!.started -= DropInput;
            _input.Player.SlowDrop!.performed -= SlowDropInput;
            _input.Player.SlowDrop!.canceled -= SlowDropInput;
            _input.Player.Rotate!.started -= RotateInput;
            
            _input.Disable();
        }

        private void Start()
        {
            BoardVisualizer.Instance?.SetBoard(_board);
        }

        private void Update()
        {
            _tickRateTimer += Time.deltaTime;
            if(_tickRateTimer >= _tickRateTime)
            {
                _tickRateTimer = 0f;
                _board.Tick();
            }
        }

        private void MovementInput(InputAction.CallbackContext context)
        {
            _board.MoveBlock(new Vector2Int((int) context.ReadValue<float>(), 0));
        }
        
        private void DropInput(InputAction.CallbackContext context)
        {
            
        }
        
        private void SlowDropInput(InputAction.CallbackContext context)
        {
            _board.MoveBlock(new Vector2Int(0, -1));
        }

        private void RotateInput(InputAction.CallbackContext context)
        {
            _board.RotateBlock((int) context.ReadValue<float>());
        }
    }
}