namespace WaveApp.Core.Entities;

public class PageInfo
{
    public int Id { get; set; }
    public string Slug { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}
