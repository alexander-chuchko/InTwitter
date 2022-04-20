using System.Collections.Generic;
using InTwitter.Controls.StateContainer.Animation;
using Xamarin.Forms;

namespace InTwitter.Controls.StateContainer
{
    [ContentProperty("Conditions")]
    public class StateContainer : ContentView
    {
        private FadeOutAnimation _disappearingAnimation;
        private FadeInAnimation _appearingAnimation;

        public StateContainer()
        {
            _disappearingAnimation = new FadeOutAnimation();
            _appearingAnimation = new FadeInAnimation();
        }

        public static void Init()
        {
            //for linker
        }

        #region -- Public Properties --

        public List<StateCondition> Conditions { get; set; } = new List<StateCondition>();

        public static readonly BindableProperty StateProperty = BindableProperty.Create(
            propertyName: nameof(State),
            returnType: typeof(object),
            declaringType: typeof(ClickableContentView),
            propertyChanged: StateChanged);

        public object State
        {
            get => GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static readonly BindableProperty HasAnimationProperty = BindableProperty.Create(
            propertyName: nameof(HasAnimation),
            returnType: typeof(bool),
            declaringType: typeof(ClickableContentView),
            defaultValue: true);

        public bool HasAnimation
        {
            get => (bool)GetValue(HasAnimationProperty);
            set => SetValue(HasAnimationProperty, value);
        }

        #endregion

        #region -- Private Helpers --

        private static void StateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
            {
                return;
            }

            var parent = bindable as StateContainer;
            parent?.ChooseStateProperty(newValue);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            ChooseStateProperty(State);
        }

        private void ChooseStateProperty(object newValue)
        {
            if (newValue == null)
            {
                return;
            }

            foreach (StateCondition stateCondition in Conditions)
            {
                if (stateCondition.Is != null)
                {
                    var splitIs = stateCondition.Is.ToString().Split(',');
                    foreach (var conditionStr in splitIs)
                    {
                        if (conditionStr.Equals(newValue.ToString()))
                        {
                            if (Content != null)
                            {
                                stateCondition.Disappearing = _disappearingAnimation;
                                if (HasAnimation)
                                {
                                    stateCondition.Disappearing?.Apply(Content);
                                }
                            }

                            Content = stateCondition.Content;
                            stateCondition.Appearing = _appearingAnimation;

                            if (HasAnimation)
                            {
                                stateCondition.Appearing?.Apply(Content);
                            }
                        }
                    }
                }
                else if (stateCondition.IsNot != null)
                {
                    if (!stateCondition.IsNot.ToString().Equals(newValue.ToString()))
                    {
                        Content = stateCondition.Content;
                    }
                }
            }
        }

        #endregion
    }
}