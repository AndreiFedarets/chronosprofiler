namespace Adenium.Menu
{
    public sealed class MenuControlStub : PropertyChangedBaseEx, IMenuControl
    {
        private readonly string _id;

        public MenuControlStub(string id)
        {
            _id = id;
        }

        public string Id
        {
            get { return _id; }
        }
        
        public bool? IsEnabled
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool? IsVisible
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
