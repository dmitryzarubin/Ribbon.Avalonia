using System;
using System.Collections.ObjectModel;

namespace Ribbon.Avalonia.Sample.ViewModels;

public class RecentDocumentViewModel
{
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime LastViewDate { get; set; }
}

public class RecentDocumentListViewModel
{
    public ObservableCollection<RecentDocumentViewModel> RecentDocuments { get; } = new()
    {
        new RecentDocumentViewModel
        {
            Name = "Document 1",
            Path = @"C:\Documents\Document1.doc",
            LastViewDate = DateTime.Now - TimeSpan.FromDays(1)
        },
        new RecentDocumentViewModel
        {
            Name = "Document 2",
            Path = @"C:\Documents\Document3.doc",
            LastViewDate = DateTime.Now - TimeSpan.FromDays(2)
        },
        new RecentDocumentViewModel
        {
            Name = "Document 3",
            Path = @"C:\Documents\Document3.doc",
            LastViewDate = DateTime.Now - TimeSpan.FromDays(5).Add(TimeSpan.FromHours(4))
        }
    };
}