using Godot;
using System;
using System.Drawing.Imaging;
using Global.Data;
using Global.Data.EffectSystem;
using Global.GameSystem;

namespace HippieFall.Game
{
    public class GameCamera : Godot.Camera, IEffectable
    {
        private Player _player;
        private float _speedProportion = 1 / 1.5f;
        private float yBackPosition = 8.8f;
        private float rotateXAngle = 0.001f;
        private float rotateZAngle = 0.001f;
        private float moveAngleX = 20 / 50f;
        private float moveAngleZ = 30 / 50f;
        private float PhysicsDelta;
        private float FovDelta;
        private float NearDelta;
        private float SizeDelta;
        private float FarDelta;
        public GameCameraConfig Config;

        private GameCameraEffectController _effectController;
        
        public override void _Ready()
        {
            Config = new GameCameraConfig(this);
            HippieFallUtilities.ConnectFeedbackAfterGameReadiness(this);
        }

        private void Init()
        {
            _player = HippieFallUtilities.Game.Player;
            _effectController = new GameCameraEffectController();
            _effectController.Init(this, Config);
        }

        public override void _PhysicsProcess(float delta)
        {
            PhysicsDelta = delta;
            MoveToPlayer();
            RotateCameraToPlayerDirection();
            SmoothChangeCameraCharacteristics();
        }
        
        private void MoveToPlayer()
        {
            Vector2 button = _player.PlayerMovementController.Joystick.GetButtonPosition();

            if(Config.DistanceKoef - Config.MaxDistanceKoef*button.Length()/64 > 0)
                Config.DistanceKoef -= PhysicsDelta;
            else Config.DistanceKoef += PhysicsDelta/2;
                
            Config.DistanceKoef = Mathf.Clamp(Config.DistanceKoef, 0.01f, Config.MaxDistanceKoef);
            Translation = new Vector3(
                _player.Translation.x * _speedProportion,
                _player.Translation.y + Config.DistanceKoef + yBackPosition - _player.Translation.y,
                _player.Translation.z * _speedProportion);
        }


        private void RotateCameraToPlayerDirection()
        {
            Vector2 move = _player.PlayerMovementController.Joystick.GetValue();
            Vector2 button = _player.PlayerMovementController.Joystick.GetButtonPosition();
            move.y *= -1;
            if (move.x > 0) moveAngleZ = 20f;
            if (move.x < 0) moveAngleZ = -20f;
            if (move.y > 0) moveAngleX = 20f;
            if (move.y < 0) moveAngleX = -20f;
            
            moveAngleX *= Mathf.Abs(move.y) * button.Length() / 64 * PhysicsDelta;
            moveAngleZ *= Mathf.Abs(move.x) * button.Length() / 64 * PhysicsDelta;
            if (rotateXAngle + moveAngleX < Config.MaxAngleX && rotateXAngle + moveAngleX > -Config.MaxAngleX)
                rotateXAngle += moveAngleX;
            else
            {
                moveAngleX *= 0;
            }

            if (rotateZAngle + moveAngleZ < Config.MaxAngleZ && rotateZAngle + moveAngleZ > -Config.MaxAngleZ)
                rotateZAngle += moveAngleZ;
            else
            {
                moveAngleZ *= 0;
            }
            
            RotationDegrees = new Vector3(rotateXAngle + (-90), 0, rotateZAngle);
        }

        private void SmoothChangeCameraCharacteristics()
        {
            if (Mathf.Abs(Fov - Config.Fov) > 0.01f)
            {
                Far += FarDelta;
                Fov += FovDelta;
                Near += NearDelta;
                Size += SizeDelta;
            }
        }
        public void ChangeConfigData(Config config)
        {
            if (config is GameCameraConfig gameCameraConfig)
            {
                Config = gameCameraConfig;
                
                FovDelta = Mathf.Abs(Fov - Config.Fov) / (0.4f/ PhysicsDelta);
                if (Fov > Config.Fov) FovDelta *= -1;

                NearDelta = Mathf.Abs(Near - Config.Near) / (0.4f/PhysicsDelta);
                if (Near > Config.Near) NearDelta *= -1;

                FarDelta = Mathf.Abs(Far - Config.Far) / (0.4f / PhysicsDelta);
                if (Far > Config.Far) FarDelta *= -1;

                SizeDelta = Mathf.Abs(Size - Config.Size) / (0.4f / PhysicsDelta);
                if (Size > Config.Size) SizeDelta *= -1;
            }
        }
    }
}