using System.ComponentModel;

namespace Frontend
{
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method is responsible for updating the screen with changes in source.
        /// </summary>
        /// <param name="property">A string of the field that was updated</param>
        /// <returns>void</returns>*/
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
