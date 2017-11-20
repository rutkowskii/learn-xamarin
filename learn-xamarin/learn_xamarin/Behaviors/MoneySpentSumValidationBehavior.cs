using Xamarin.Forms;

namespace learn_xamarin.Behaviors
{
    public class MoneySpentSumValidationBehavior : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(MoneySpentSumValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = ResolveIfValid(e.NewTextValue);
        }

        private bool ResolveIfValid(string textValue)
        {
            double parsingResult = 0.0;
            if (!double.TryParse(textValue, out parsingResult))
            {
                return false;
            }
            if (parsingResult <= 0) return false;
            return true;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
        }
    }
}
