namespace RWS
{
    public class SensitivityTabState : BaseState<SettingsPanel>
    {
        public override void OnEnableState()
        {
            owner.sensitivityPanel.OnBackButtonClicked += OnBackButtonClicked;
            owner.sensitivityPanel.Show();
        }

        public override void OnDisableState()
        {
            owner.sensitivityPanel.Hide();
            owner.sensitivityPanel.OnBackButtonClicked -= OnBackButtonClicked;
        }
        
        
        void OnBackButtonClicked()
        {
            owner.ChangeState( new NavigationTabState() );
        }
    }
}

