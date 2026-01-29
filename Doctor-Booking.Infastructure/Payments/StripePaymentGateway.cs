using Doctor_Booking.Application.Common.Payment;
using Doctor_Booking.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Doctor_Booking.Infastructure.Payments;

public class StripePaymentGateway : IPaymentGateway, IPaymentRefundGateway
{
	public StripePaymentGateway(IConfiguration configuration)
	{
		StripeConfiguration.ApiKey =
			configuration["Stripe:SecretKey"];
	}
	public async Task<string> CreatePaymentAsync(
	decimal amount,
	string currency,
	string description,
	string paymentMethodId)
	{
		// 1️⃣ Create PaymentIntent options
		var options = new PaymentIntentCreateOptions
		{
			Amount = (long)(amount * 100), // Stripe works in cents
			Currency = currency,
			Description = description,

			// بدل token
			PaymentMethod = paymentMethodId,

			// تأكيد الدفع فورًا
			Confirm = true,

			// دعم 3D Secure وطرق دفع تلقائي
			AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
			{
				Enabled = true ,
				AllowRedirects = "never"
			}
		};

		// 2️⃣ Create PaymentIntent
		var service = new PaymentIntentService();
		var paymentIntent = await service.CreateAsync(options);

		// 3️⃣ Check result
		if (paymentIntent.Status != "succeeded")
		{
			throw new Exception(
				$"Stripe payment failed with status: {paymentIntent.Status}"
			);
		}

		// 4️⃣ Transaction Reference (تحفظه في DB)
		return paymentIntent.Id;        // pi_xxxxx

	}

	public async  Task<RefundResult> RefundAsync
		(string transactionRef, decimal amount, CancellationToken cancellationToken)
	{
		try
		{
			var refundService = new RefundService();

			var options = new RefundCreateOptions
			{
				PaymentIntent = transactionRef
				// Amount omitted = full refund
			};

			var refund = await refundService.CreateAsync(
				options,
				requestOptions: null,
				cancellationToken: cancellationToken
			);

			if (refund.Status == "succeeded")
			{
				return RefundResult.Ok(refund.Id);
			}

			return RefundResult.Fail(
				$"Refund failed with status: {refund.Status}"
			);
		}
		catch (StripeException ex)
		{
			return RefundResult.Fail(
				ex.StripeError?.Message ?? ex.Message
			);
		}
		catch (Exception ex)
		{
			return RefundResult.Fail(ex.Message);
		}
	}
}
	

