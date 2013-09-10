using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using PastaGameLibrary;

namespace TestBed
{
	//public class BoughtUpgrade
	//{
	//    enum BoughtUpgradeState
	//    {
	//        Waiting,
	//        Moving,
	//        TrumpetsRaising,
	//        Idle,
	//    }

	//    const float WaitTime = 1.2f;
	//    const float MoveTime = 0.5f;
	//    const float TrumpetTime = 0.2f;
	//    Vector2 Target = new Vector2(-120, -25);
	//    const float FinalScale = 1.5f;
        
	//    const double BaseAngle = 1.0f;
	//    int BaseDistance = 50;

	//    float _baseScale;
	//    Vector2 _basePosition;

	//    float _raiseTimer;
	//    Transform m_transform;
	//    Sprite m_upgrade, m_background;
	//    Sprite m_leftGhost, m_rightGhost;
	//    Animation m_bgAnimation;
	//    ParticleSystem m_particleSystem;
	//    ShopScene m_shopScene;
	//    BoughtUpgradeState m_currentState;
	//    ConfettiGenerator _generatorRight, _generatorLeft;

	//    public BoughtUpgrade(UpgradeButton upgradeButton, ShopScene shopScreen) 
	//        : base()
	//    {
	//        m_shopScene = shopScreen;
	//        m_transform = new Transform(m_shopScene.Hotspot, true);
	//        m_particleSystem = new ParticleSystem(Globals.TheGame, 100);

	//        m_upgrade = new Sprite(Globals.TheGame, upgradeButton.Upgrade.GetTexture(), m_transform);
	//        m_background = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("upgrade_bg", 1, 6), new Transform(m_transform, true));
	//        m_background.Transform.Scale = Vector2.Zero;
	//        m_bgAnimation = new Animation(m_background, 0, 5, 0.2f, true);

	//        m_leftGhost = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("trumpetghost_r"), new Transform(m_transform, true));
	//        _generatorRight = new ConfettiGenerator(m_particleSystem);
	//        _generatorRight.Colors = ConfettiGenerator.GhostConfettiColors;
	//        _generatorRight.Transform.Direction = -0.75f;
	//        _generatorRight.Spread = 0.25f;
	//        _generatorRight.Transform.ParentTransform = m_leftGhost.Transform;
	//        _generatorRight.Transform.Position = new Vector2(15, -25);

	//        m_rightGhost = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("trumpetghost_l"), new Transform(m_transform, true));
	//        _generatorLeft = new ConfettiGenerator(m_particleSystem);
	//        _generatorLeft.Colors = ConfettiGenerator.GhostConfettiColors;
	//        _generatorLeft.Transform.Direction = Math.PI + 0.75f;
	//        _generatorLeft.Spread = 0.25f;
	//        _generatorLeft.Transform.ParentTransform = m_rightGhost.Transform;
	//        _generatorLeft.Transform.Position = new Vector2(-15, -25);

	//        BaseDistance = (int)(m_upgrade.Width * 0.5f * FinalScale + m_leftGhost.Width * 0.5f);

	//        _basePosition = upgradeButton.Transform.Position + new Vector2(0, -42); //Slight adjustment to remove harsh transition
	//        m_transform.Position = _basePosition;
	//        _baseScale = upgradeButton.Transform.Scale;
	//        m_transform.Scale = _baseScale;

	//        SetWait();
	//    }

	//    private void SetWait()
	//    {
	//        m_currentState = BoughtUpgradeState.Waiting;
	//        _raiseTimer = WaitTime;
	//        //m_leftGhost.Hide();
	//        //m_rightGhost.Hide();
	//        //m_background.Hide();
	//       // _buyCoins.Generate(25);
	//    }
	//    private void SetMoving()
	//    {
	//        _raiseTimer = MoveTime;
	//        m_currentState = BoughtUpgradeState.Moving;        
	//        //m_background.Show();
	//    }
	//    private void SetTrumpetsRaising()
	//    {
	//        _raiseTimer = TrumpetTime;
	//        m_currentState = BoughtUpgradeState.TrumpetsRaising;
	//     //   m_leftGhost.Show();
	//       // m_rightGhost.Show();
	//    }
	//    private void SetIdle()
	//    {
	//        _raiseTimer = 0;
	//        m_currentState = BoughtUpgradeState.Idle;
	//        _generatorRight.Generate(50, new object[] { m_particleSystem });
	//        _generatorLeft.Generate(50, new object[] { m_particleSystem });
	//    }

	//    private void UpdateWait()
	//    {
	//        if (_raiseTimer == 0)
	//            SetMoving();
	//    }
	//    private void UpdateIdle()
	//    {
	//        if (_raiseTimer <= 0)
	//            _raiseTimer += (float)Math.PI * 2;
	//        float displacement = (float)Math.Sin(_raiseTimer) * 5;
	//        m_transform.PosY = Target.Y + displacement;
	//    }
	//    private void UpdateMoving()
	//    {
	//        float ratio = 1 - _raiseTimer / MoveTime;
	//        m_transform.SclX = m_transform.SclY = _baseScale + (FinalScale - _baseScale) * ratio;
	//        m_transform.Position = _basePosition + (Target - _basePosition) * ratio;
	//        m_background.Transform.SclX = m_background.Transform.SclY = FinalScale * ratio;
	//        if (_raiseTimer == 0)
	//            SetTrumpetsRaising();
	//    }
	//    private void UpdateTrumpets()
	//    {
	//        float ratio = 1 - _raiseTimer / TrumpetTime;
	//        m_rightGhost.Transform.PosX = -BaseDistance * ratio;
	//        m_rightGhost.Transform.Direction = -BaseAngle + BaseAngle * ratio;
	//        m_leftGhost.Transform.PosX = BaseDistance * ratio;
	//        m_rightGhost.Transform.Direction = BaseAngle - BaseAngle * ratio;
	//        m_rightGhost.Alpha = m_leftGhost.Alpha = Math.Min(ratio * 2, 1);
	//        if (_raiseTimer == 0)
	//            SetIdle();
	//    }

	//    public override void Update(GameTime gameTime)
	//    {
	//        m_bgAnimation.Update();
	//        _raiseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
	//        if (_raiseTimer < 0)
	//            _raiseTimer = 0;

	//        switch (m_currentState)
	//        {
	//            case BoughtUpgradeState.Waiting:
	//                UpdateWait();
	//                break;
	//            case BoughtUpgradeState.Idle:
	//                UpdateIdle();
	//                break;
	//            case BoughtUpgradeState.Moving:
	//                UpdateMoving();
	//                break;
	//            case BoughtUpgradeState.TrumpetsRaising:
	//                UpdateTrumpets();
	//                break;
	//        }
	//        //_buyCoins.Update(gameTime);
	//        m_particleSystem.Update();
	//    }
	//    public override void Draw(SpriteBatch spriteBatch)
	//    {
	//        //_buyCoins.Draw(spriteBatch);
	//        if (m_currentState == BoughtUpgradeState.Waiting)
	//            return;

	//        m_background.Draw();
	//        if (m_currentState == BoughtUpgradeState.TrumpetsRaising
	//            || m_currentState == BoughtUpgradeState.Idle)
	//        {
	//            m_leftGhost.Draw();
	//            m_rightGhost.Draw();
	//        }
	//        m_particleSystem.Draw();
	//    }
	//}
	//public class SelectionCursor
	//{
	//    Vector2 m_target;
	//    ShopScene m_shopScene;
	//    Transform m_transform;
	//    Sprite m_sprite;

	//    public SelectionCursor(ShopScene parent)
	//        : base()
	//    {
	//        m_transform = new Transform(parent.Hotspot);
	//        m_sprite = new Sprite(Globals.TheGame, InsoGame.Pixel, m_transform);
	//        m_shopScene = parent;
	//        m_sprite.Colour = new Vector4(0, 0, 0, 0.75f);
	//    }

	//    public void SetTarget(Vector2 target)
	//    {
	//        m_target = target;
	//    }

	//    public void Update()
	//    {
	//        m_transform.Position += (m_target - m_transform.Position) * 0.25f;
	//    }

	//    public void Draw()
	//    {
	//        m_sprite.Draw();
	//    }
	//}
	//public class UpgradeButton : Button
	//{
	//    const float TopAcceleration = -3.0f;
	//    float _topVelocity = 10f;

	//    bool _hasBeenBought = false;
	//    Counter _price;
	//    Upgrade _upgrade;
	//    ShopScene _shopScreen;

	//    const float BaseScale = 0.8f;
	//    float _scaleTarget = BaseScale, _currentScale;

	//    public float Scale
	//    {
	//        get { return m_buttonSprite.Scale.X; }
	//    }

	//    public void Hide()
	//    {
	//        m_buttonSprite.Hide();
	//    }

	//    public Upgrade Upgrade
	//    {
	//        get { return _upgrade; }
	//    }
	//    public string Description
	//    {
	//        get { return _upgrade.Description; }
	//    }

	//    public UpgradeButton(ShopScene parent, Upgrade upgrade)
	//        : base(upgrade.GetTexture(), 1, 1)
	//    {
	//        _currentScale = _scaleTarget;
	//        _upgrade = upgrade;
	//        _shopScreen = parent;

	//        _price = new Counter(_upgrade.Price.ToString().Length, TextureLibrary.Get("counter_vertical"));
	//        _price.ForceValue(_upgrade.Price);
	//        _price.BindParent(this);
	//        _price.Y = 15;
	//        _price.X -= 3;
	//        _price.Alpha = 1;
	//        BindParent(_shopScreen.Hotspot);
	//        m_buttonSprite.Y -= m_buttonSprite.Width * 0.5f;
	//        m_buttonSprite.ScaleUniform(0.8f);
	//    }

	//    public override void Update(GameTime gameTime)
	//    {
	//        base.Update(gameTime);
	//        _currentScale += (_scaleTarget - _currentScale) * 0.5f;
	//        m_buttonSprite.ScaleUniform(_currentScale);
	//        _price.Update(gameTime);

	//        if (m_currentState == ButtonState.Locked)
	//        {
	//            Y += _topVelocity;
	//            _topVelocity += TopAcceleration;
	//        }
	//    }

	//    public override void OnPush()
	//    {
	//        if (_hasBeenBought)
	//            return;
	//        if (m_currentState == ButtonState.Released)
	//            _scaleTarget = BaseScale + 0.1f;
	//        m_currentState = ButtonState.Pushed;
	//    }
	//    public override void OnRelease()
	//    {
	//        if (_hasBeenBought)
	//            return;

	//        if (m_currentState == ButtonState.Pushed)
	//            _scaleTarget = BaseScale;
	//        m_currentState = ButtonState.Released;
	//    }

	//    public void Deselect()
	//    {
	//        _scaleTarget = BaseScale;
	//    }

	//    public override void Lock()
	//    {
	//        base.Lock();
	//        _scaleTarget = 0.8f;
	//        m_buttonSprite.Color = new Color(50, 50, 50);
	//        _price.Visible = false;
	//    }

	//    public override void OnValidate()
	//    {
	//        base.OnValidate();
	//        if (_shopScreen.IsButtonSelected(this))
	//            BuyUpgrade();
	//        else
	//        {
	//            _scaleTarget = 1.0f;
	//            _shopScreen.SelectButton(this);
	//        }
	//    }
	//    private void BuyUpgrade()
	//    {           
	//        _price.Visible = false;
	//        _hasBeenBought = true;
	//        _scaleTarget = 1.5f;
	//        _shopScreen.BuyUpgrade();
	//        GameScene.GameChest.RemoveFromChest(_upgrade.Price);
	//        _upgrade.DoUpgrade();
	//        _shopScreen.AddUpgradeToBar(_upgrade);
	//    }
	//    public void UpdateButtonStatus()
	//    {
	//        if (GameScene.GameChest.PlayerMoney < _upgrade.Price)
	//            Lock();
	//    }

	//    public override void Draw(SpriteBatch spriteBatch)
	//    {
	//        base.Draw(spriteBatch);
	//        _price.Draw(spriteBatch);
	//    }
	//}
	//public class DescriptionText : Entity
	//{
	//    const float TotalDisplayTime = 1.0f;
	//    TextActor _text;
	//    string _stockText;
	//    float _letterDisplayInterval, _letterDisplayTimer;

	//    public float Alpha
	//    {
	//        set { _text.Alpha = value; }
	//    }

	//    public DescriptionText()
	//        : base()
	//    {
	//        _text = new TextActor(SpritefontManager.Get("description"));
	//        _text.Text = "";
	//        _text.BindParent(this);
	//        _stockText = "";

	//        _text.TextColor = Color.White;
	//        _text.OriginRatio = new Vector2(0, 0);
	//        _text.Position = new Vector2(15, 10);     
	//    }

	//    public void ScaleUniform(float scale)
	//    {
	//        _text.ScaleUniform(scale);
	//    }
	//    public void SetNewText(string text)
	//    {
	//        _stockText = text;
	//        _letterDisplayInterval = TotalDisplayTime / _stockText.Length;
	//        _letterDisplayTimer = _letterDisplayInterval;
	//        _text.Text = "";
	//    }

	//    public void Update(GameTime gameTime)
	//    {
	//        _letterDisplayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

	//        while (_letterDisplayTimer < 0
	//            && _text.Text.Length != _stockText.Length)
	//        {
	//            _letterDisplayTimer += _letterDisplayInterval;
	//            _text.Text = _stockText.Substring(0, _text.Text.Length + 1);
	//        }
	//    }
	//    public void Draw(SpriteBatch spriteBatch)
	//    {
	//        _text.Draw(spriteBatch);
	//    }
	//}
	//public class Description : Entity
	//{
	//    const float ScaleTime = 0.3f;
	//    const float BaseAlpha = 0.5f;

	//    bool _isActivated, _isScaling;
	//    DescriptionText _descriptionText;
	//    float _scaleTimer;
	//    Sprite _background;
	//    ShopScene _shopScreen;

	//    public Description(ShopScene shopScreen)
	//        : base()
	//    {
	//        _shopScreen = shopScreen;
	//        BindParent(shopScreen.Hotspot);

	//        _descriptionText = new DescriptionText();
	//        _descriptionText.BindParent(this);
	//        _descriptionText.ScaleUniform(0);

	//        _background = new Sprite(TextureLibrary.GetSpriteSheet("description_bg"));
	//        _background.Origin = new Vector2(0, 30);
	//        _background.BindParent(this);
	//        _background.ScaleUniform(0);

	//        Position = new Vector2(-360, 30);
	//    }

	//    public void ActivateDescription(string descriptionText)
	//    {
	//        if (!_isActivated)
	//            _scaleTimer = ScaleTime;

	//        _isActivated = _isScaling = true;
	//        _descriptionText.SetNewText(descriptionText);
	//    }
	//    public void DeactivateDescription()
	//    {
	//        _isActivated = false;
	//        _isScaling = true;
	//        _scaleTimer = ScaleTime;
	//    }

	//    public void Update(GameTime gameTime)
	//    {
	//        if (_isScaling)
	//        {
	//            _scaleTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
	//            if (_scaleTimer < 0)
	//                _scaleTimer = 0;
	//            float ratio = _scaleTimer / ScaleTime;
	//            if (_isActivated)
	//                ratio = 1 - ratio;

	//            _descriptionText.ScaleUniform(ratio);
	//            _descriptionText.Alpha = ratio;
	//            _background.ScaleUniform(ratio);
	//            _background.Alpha = ratio * BaseAlpha;

	//            if (_scaleTimer == 0)
	//                _isScaling = false;
	//        }
	//        _descriptionText.Update(gameTime);
	//    }
	//    public void Draw(SpriteBatch spriteBatch)
	//    {
	//        _background.Draw(spriteBatch);
	//        _descriptionText.Draw(spriteBatch);
	//    }
	//}

	//public class ShopScene : DynamicMenuScene
	//{
	//    Song _shopSong;

	//    int _amountOfSlots = 2;
	//    bool _hasBeenInitialised = false;
	//    UpgradeButton[] _upgradeButtons;
	//    UpgradeButton _selectedButton;
	//    BoughtUpgrade _boughtUpgrade;
	//    UpgradeBar _possessedUpgrades;

	//    Sprite _descriptionBackground;
	//    SelectionCursor _selectionCursor;
	//    Description _description;

	//    public bool HasBeenInitialised
	//    {
	//        get { return _hasBeenInitialised; }
	//        set { _hasBeenInitialised = value; }
	//    }

	//    public ShopScene()
	//        : base()
	//    {
	//        _descriptionBackground = new Sprite(InsoGame.Pixel);
	//        _descriptionBackground.Origin = new Vector2(0.5f, 0.5f);
	//        _selectionCursor = new SelectionCursor(this);
	//        _possessedUpgrades = new UpgradeBar(this);
	//        _description = new Description(this);
	//        _shopSong = MusicBank.GetSong("demo_Latino_Flute_Full_Track_");
	//    }

	//    public override void Transition(Scene.ScreenState stateToTransitionTo, Scene otherScreen)
	//    {
	//        base.Transition(stateToTransitionTo, null);

	//        if (stateToTransitionTo == ScreenState.Active)
	//        {
	//            if(!_hasBeenInitialised)
	//                Initialise();
	//            GameScene.GameChest.PopInSky();
	//            if(_boughtUpgrade == null)
	//                _selectionCursor.Show();
	//            _possessedUpgrades.PopOut();
	//            MediaPlayer.Play(_shopSong);
	//            MediaPlayer.Volume = 1;
	//        }
	//        if (stateToTransitionTo == ScreenState.Inactive)
	//        {
	//            //if (_boughtUpgrade == null)
	//            //{
	//            //    ShopOutroNoBuy();
	//            //}
	//            //else
	//            //{
	//            //    ShopOutroBuy();
	//            //}
	//            _possessedUpgrades.PopIn();
	//            MediaPlayer.Pause();
	//        }
	//    }
	//    protected override void UpdateTransition(GameTime gameTime)
	//    {
	//        if (_nextState == ScreenState.Inactive)
	//        {
	//            _possessedUpgrades.Update(gameTime);
	//            if (_possessedUpgrades.IsIn)
	//                _screenState = ScreenState.Inactive;
	//        }
	//        else
	//            _screenState = ScreenState.Active;
	//    }

	//    public void UpgradeAmountOfSlots()
	//    {
	//        _amountOfSlots++;
	//    }

	//    public void ResetShopScreen()
	//    {
	//        _hasBeenInitialised = false;
	//        _boughtUpgrade = null;
	//    }
	//    protected override void UpdateActive(GameTime gameTime)
	//    {
	//        for(int i = 0; i < _upgradeButtons.Length; ++i)
	//            if(_upgradeButtons[i] != null)
	//             _upgradeButtons[i].Update(gameTime);
	//        _possessedUpgrades.Update(gameTime);
	//        _selectionCursor.Update(gameTime);
	//        _description.Update(gameTime);
	//    }

	//    public void SelectButton(UpgradeButton selectedButton)
	//    {
	//        if (_selectedButton != null)
	//            _selectedButton.Deselect();
	//        _selectedButton = selectedButton;
	//        _selectionCursor.SetTarget(_selectedButton.Position + new Vector2(0, -25));
	//        _description.ActivateDescription(_selectedButton.Description);
	//    }
	//    public bool IsButtonSelected(UpgradeButton button)
	//    {
	//        return _selectedButton == button;
	//    }
	//    public void AddUpgradeToBar(Upgrade upgrade)
	//    {
	//        _possessedUpgrades.Add(upgrade);
	//    }
	//    public void RemoveSelected()
	//    {
	//        for (int i = 0; i < _upgradeButtons.Length; ++i)
	//            if (_upgradeButtons[i] == _selectedButton)
	//                _upgradeButtons[i] = null;
	//    }
	//    public void BuyUpgrade()
	//    {
	//        _boughtUpgrade = new BoughtUpgrade(_selectedButton, this);
	//        INSOSceneManager.GameScreen.SetNewBoughtUpgrade(_boughtUpgrade);
	//        for (int i = 0; i < _upgradeButtons.Length; ++i)
	//            _upgradeButtons[i].Lock();
	//        _selectedButton.Hide();
	//        _selectionCursor.Hide();
	//        _description.DeactivateDescription();
	//    }

	//    public void Initialise()
	//    {
	//        _hasBeenInitialised = true;
	//        Upgrade[] selectedUpgrades = GameScene.UpgradeManager.FindUpgrades(_amountOfSlots);
	//        _upgradeButtons = new UpgradeButton[selectedUpgrades.Length];
	//        for (int i = 0; i < _upgradeButtons.Length; ++i)
	//        {
	//            _upgradeButtons[i] = new UpgradeButton(this, selectedUpgrades[i]);
	//            _upgradeButtons[i].BindParent(Hotspot);
	//            _upgradeButtons[i].X = -200 + 100 * i;
	//            _upgradeButtons[i].Y = -100;
	//        }
	//        SelectButton(_upgradeButtons[0]);
	//    }
	//    public void UpdateShop()
	//    {
	//        for (int i = 0; i < _upgradeButtons.Length; ++i)
	//            _upgradeButtons[i].UpdateButtonStatus();
	//    }

	//    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
	//    {
	//        if(_upgradeButtons == null)
	//            return;
	//        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, World.cam_Main.CameraMatrix);
	//        _description.Draw(spriteBatch);
	//        _selectionCursor.Draw(spriteBatch);
	//        for (int i = 0; i < _upgradeButtons.Length; ++i)
	//            if (_upgradeButtons[i] != null)
	//                _upgradeButtons[i].Draw(spriteBatch);
	//        _possessedUpgrades.Draw(spriteBatch);
	//        spriteBatch.End();
	//    }
	//}
}
