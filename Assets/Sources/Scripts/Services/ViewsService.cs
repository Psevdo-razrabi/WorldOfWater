﻿using System;
using Game.MVVM;
using System.Collections.Generic;
using Game.MVVM.Menu;

namespace Game.Services
{
    public class ViewsService
    {
        private readonly ViewFactory _viewFactory;
        private readonly Dictionary<Type, View> _views = new();
        private readonly Stack<View> _viewsStack = new();

        public ViewsService(ViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }
        
        public void Initialize()
        {
            _viewFactory.Initialize();
            Open<MainMenuView>();
        }

        public async void Open<T>() where T : View
        {
            if (_views.TryGetValue(typeof(T), out var view))
            {
                view.gameObject.SetActive(true);
                view.Open();
                _viewsStack.Push(view);
            }
            else
            {
                var newView = _viewFactory.Create<T>();
                newView.gameObject.SetActive(true);
                newView.Open();
                _views.Add(typeof(T), newView);
                _viewsStack.Push(newView);
            }
        }

        public void Close()
        {
            if (_viewsStack.TryPop(out var view))
            {
                view.Close();
                view.gameObject.SetActive(false);
            }
        }
    }
}
