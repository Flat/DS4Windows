﻿using DS4Windows.Client.Core.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DS4Windows.Client.Core.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public List<INavigationTabViewModel> CreateNavigationTabViewModels()
        {
            var navigationTabViewModels = serviceProvider.GetServices<INavigationTabViewModel>();

            foreach (var navigationTabViewModel in navigationTabViewModels)
            {
                var tabView = serviceProvider.GetService(navigationTabViewModel.GetViewType());
                Initialize(navigationTabViewModel, tabView);
            }

            return navigationTabViewModels
                .OrderBy(vm => vm.TabIndex)
                .ToList();
        }

        public TViewModel Create<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView
        {
            var viewModel = CreateViewModel<TViewModel>();
            var view = CreateView<TView>();

            Initialize(viewModel, view);

            return viewModel;
        }

        public TViewModel CreateViewModel<TViewModel>()
            where TViewModel : IViewModel
        {
            return serviceProvider.GetService<TViewModel>();
        }

        public TView CreateView<TView>()
            where TView : IView
        {
            return serviceProvider.GetService<TView>();
        }

        private void Initialize(object viewModel, object view)
        {
            var internalViewModel = (IViewModel)viewModel;
            var internalView = (IView)view;

            internalViewModel.AddView(internalView);
            internalView.DataContext = internalViewModel;
        }
    }
}
