using CommunityToolkit.Mvvm.ComponentModel;
/// <summary>
/// Model cho các lựa chọn trong câu hỏi multiple choice
/// </summary>
/// <remarks>
/// Quản lý:
/// - Nội dung lựa chọn
/// - Trạng thái được chọn
/// - Trạng thái đúng/sai
/// </remarks>
public class QuestionOptionModel : ObservableObject
{
    private string _text;
    private bool _isSelected;
    private bool _isCorrect;
    private bool _isWrong;

    public string Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public bool IsCorrect
    {
        get => _isCorrect;
        set => SetProperty(ref _isCorrect, value);
    }

    public bool IsWrong
    {
        get => _isWrong;
        set => SetProperty(ref _isWrong, value);
    }
} 