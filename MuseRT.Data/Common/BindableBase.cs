using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MuseRT.Data
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> 
        /// Initializes the bindable object. 
        /// </summary> 
        /// <param name="parameters">Dictionary with the parameters.</param> 
        public virtual void Initialize(IDictionary<string, string> parameters)
        {
        }

        /// <summary> 
        /// Event handler for the PropertyChanged event. 
        /// </summary> 
        /// <param name="propertyName">The name of the property that has changed.</param> 
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                try
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch { }
            }

        }

        /// <summary> 
        /// Sets the value of the binded property. 
        /// </summary> 
        /// <typeparam name="T">The generic type.</typeparam> 
        /// <param name="storage">The type of the property.</param> 
        /// <param name="value">The value of the property.</param> 
        /// <param name="propertyName">The name of the property.</param> 
        /// <returns>A boolean indicating the success of the assignation.</returns> 
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
