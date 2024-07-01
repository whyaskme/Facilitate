namespace UploadExample.Data 
{
    public class TestModelService 
    {
        private List<TestModel> CreateDataSource() 
        {
            List<TestModel> temp = new List<TestModel>();
            temp.Add(new TestModel() { ID = 0, Name = "John", ImageUrl = @"Images\SmallGreen.jpg" });
            temp.Add(new TestModel() { ID = 1, Name = "Peter", ImageUrl = @"Images\SmallRed.jpg" });
            temp.Add(new TestModel() { ID = 2, Name = "James", ImageUrl = @"Images\SmallYellow.jpg" });
            return temp;
        }

        private List<TestModel> DataSource { get; set; }

        public TestModelService() 
        {
            DataSource = CreateDataSource();
        }

        public Task<IEnumerable<TestModel>> GetDataSourceAsync(CancellationToken ct = default) 
        {
            return Task.FromResult(DataSource.AsEnumerable());
        }

        List<TestModel> InsertInternal(TestModel itemModel) 
        {
            var dataItem = new TestModel();
            Update(dataItem, itemModel);
            dataItem.ID = DataSource.OrderBy(m => m.ID).LastOrDefault().ID + 1;
            DataSource.Add(dataItem);
            return DataSource;
        }

        public Task<List<TestModel>> Insert(TestModel itemModel) 
        {
            return Task.FromResult(InsertInternal(itemModel));
        }

        List<TestModel> RemoveInternal(TestModel dataItem) 
        {
            DataSource.Remove(dataItem);
            return DataSource;
        }

        public Task<List<TestModel>> Remove(TestModel dataItem) 
        {
            return Task.FromResult(RemoveInternal(dataItem));
        }

        List<TestModel> UpdateInternal(TestModel dataItem, TestModel itemModel) 
        {
            dataItem.Name = itemModel.Name;
            dataItem.ImageUrl = itemModel.ImageUrl;
            return DataSource;
        }

        public Task<List<TestModel>> Update(TestModel dataItem, TestModel itemModel) 
        {
            return Task.FromResult(UpdateInternal(dataItem, itemModel));
        }
    }
}
