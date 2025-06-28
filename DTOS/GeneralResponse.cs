namespace WebAPIDotNet.DTOS
{
    public class GeneralResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
