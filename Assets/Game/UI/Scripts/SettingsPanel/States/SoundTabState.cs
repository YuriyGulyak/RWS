namespace RWS
{
    public class SoundTabState : BaseState<SettingsPanel>
    {
        public override void OnEnableState()
        {
            owner.soundPanel.OnBackButtonClicked += OnBackButtonClicked;
            owner.soundPanel.Show();
        }

        public override void OnDisableState()
        {
            owner.soundPanel.Hide();
            owner.soundPanel.OnBackButtonClicked -= OnBackButtonClicked;
        }
        
        
        void OnBackButtonClicked()
        {
            owner.ChangeState( new NavigationTabState() );
        }
    }
}


