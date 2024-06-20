namespace App.Services.CouponAPI.Entities.DTO{
    public class BaseResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }
}