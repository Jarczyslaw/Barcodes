using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.Services.StateSaver
{
    public class StateSaverService : IStateSaverService
    {
        private readonly Dictionary<Type, object> savedStates = new Dictionary<Type, object>();

        public void Save<TViewModel, TState>(TViewModel viewModel)
        {
            if (savedStates.ContainsKey(typeof(TViewModel)))
            {
                savedStates.Remove(typeof(TViewModel));
            }

            savedStates.Add(typeof(TViewModel), TinyMapper.Map<TState>(viewModel));
        }

        public void Load<TViewModel, TState>(TViewModel viewModel)
        {
            if (savedStates.TryGetValue(typeof(TViewModel), out object currentState))
            {
                TinyMapper.Map((TState)currentState, viewModel);
            }
        }
    }
}
