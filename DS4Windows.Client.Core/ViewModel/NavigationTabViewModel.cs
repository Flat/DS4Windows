﻿using DS4Windows.Client.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS4Windows.Client.Core.ViewModel
{
    public abstract class NavigationTabViewModel<TViewModel, TView> : ViewModel<TViewModel>, INavigationTabViewModel<TViewModel, TView>
        where TViewModel : INavigationTabViewModel<TViewModel, TView>
        where TView : IView<TView>
    {
        public abstract int TabIndex { get; }
        public abstract string? Header { get; }

        public Type GetViewType()
        {
            return typeof(TView);
        }
    }
}
