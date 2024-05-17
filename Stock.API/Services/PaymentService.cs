using Common.Shared.DTOs;

namespace Stock.API.Services
{
	public class PaymentService
	{
		private HttpClient _httpClient;

		public PaymentService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<(bool isSucces,string? failMessage)> CreatePaymentProcess(PaymentCreateRequestDto paymentCreateRequestDto)
		{
			var response = await _httpClient.PostAsJsonAsync<PaymentCreateRequestDto>("api/PaymentProcessCreate",paymentCreateRequestDto);

			var responseContent= await response.Content.ReadFromJsonAsync<ResponseDto<PaymentCreateResponseDto>>();

			return response.IsSuccessStatusCode ?(true,null):(false,responseContent!.Errors!.First());
		}
	}
}
