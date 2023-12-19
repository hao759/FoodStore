using CuaHangDoAn.Models.Momo;
using CuaHangDoAn.Models;

namespace CuaHangDoAn.Services;

public interface IMomoService
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(Order model);
    MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
}