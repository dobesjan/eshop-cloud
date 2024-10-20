using Eshop.Api.Controllers;
using Eshop.DataAccess.Repository.Orders;
using Eshop.DataAccess.Repository.Products;
using Eshop.DataAccess.UnitOfWork;
using Eshop.Models.Api.Requests.Cart;
using Eshop.Models.Orders;
using Eshop.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Eshop.Api.UnitTests
{
	public class CartControllerTests
	{
		[Fact]
		public void GetCart_ShouldReturnOk_WhenCartExists()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockOrderRepository = new Mock<IOrderRepository>();
			mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
			var cart = new Order();
			mockOrderRepository.Setup(o => o.GetCart(It.IsAny<string>())).Returns(cart);
			var controller = new CartController(mockUnitOfWork.Object);

			var result = controller.GetCart("user123");

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(cart, okResult.Value);
		}

		[Fact]
		public void CreateCart_ShouldReturnOk_WhenCartIsCreated()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockOrderRepository = new Mock<IOrderRepository>();
			mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
			var newCart = new Order { UserId = "user123" };
			mockOrderRepository
				.Setup(o => o.Add(It.IsAny<Order>(), It.IsAny<bool>()))
				.Returns(newCart);
			var controller = new CartController(mockUnitOfWork.Object);

			var result = controller.CreateCart("user123");

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(newCart, okResult.Value);
		}

		[Fact]
		public void AddToCart_ShouldReturnOk_WhenProductIsAdded()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockOrderRepository = new Mock<IOrderRepository>();
			var mockProductRepository = new Mock<IProductRepository>();
			mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
			mockUnitOfWork.Setup(u => u.ProductRepository).Returns(mockProductRepository.Object);

			var product = new Product();
			mockProductRepository
				.Setup(p => p.Get(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
				.Returns(product);

			var cart = new Order();
			mockOrderRepository.Setup(o => o.AddToCart(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
							   .Returns(cart);

			var controller = new CartController(mockUnitOfWork.Object);

			var request = new AddToCartRequest { UserId = "user123", CartId = 1, ProductId = 1, Count = 2 };

			var result = controller.AddToCart(request);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(cart, okResult.Value);
		}

		[Fact]
		public void AddToCart_ShouldReturnBadRequest_WhenProductNotFound()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockProductRepository = new Mock<IProductRepository>();
			mockUnitOfWork.Setup(u => u.ProductRepository).Returns(mockProductRepository.Object);

			mockProductRepository
				.Setup(p => p.Get(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
				.Returns((Product)null);

			var controller = new CartController(mockUnitOfWork.Object);

			var request = new AddToCartRequest { ProductId = 1 };

			var result = controller.AddToCart(request);

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("Product not found!", ((dynamic)badRequestResult.Value).Error);
		}

		[Fact]
		public void Order_ShouldReturnOk_WhenOrderIsSuccessful()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockOrderRepository = new Mock<IOrderRepository>();
			mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);

			var paymentMethod = new PaymentMethod
			{
				Name = "Card",
				Enabled = true,
				ShippingPaymentMethod = new List<ShippingPaymentMethod>
				{
					new ShippingPaymentMethod
					{
					ShippingId = 1,
					PaymentMethodId = 1,
					}
				}
			};

			var payment = new Payment
			{
				OrderId = 1,
				PaymentStatusId = 1,
				PaymentMethodId = 1,
				PaymentMethod = paymentMethod,
				Cost = 30,
				CostWithTax = 45
			};

			var cart = new Order
			{
				Id = 1,
				CreatedDate = DateTime.UtcNow,
				OrderProducts = new List<OrderProduct>
				{
					new OrderProduct
					{
						ProductId = 1,
						Product = new Product
						{
							Name = "chair",
							Enabled = true,
							IsInStock = true,
							BuyLimit = 50,
							Cost = 10,
							CostWithTax = 15,
							CostBefore = 12,
							ImageUrl = String.Empty
						},
						OrderId = 1,
						Count = 3
					}
				},
				ShippingId = 1,
				BillingContactId = 1,
				BillingContact = new Models.Users.Contact
				{
					Id = 1,
					PersonId = 1,
					Person = new Models.Users.Person
					{
						FirstName = "Alan",
						LastName = "Wilkins",
						Email = "wilkins@gmail.com",
						PhoneNumber = "+1 256 333 222",
					},
					AddressId = 1,
					Address = new Models.Users.Address
					{
						CustomerId = 1,
						City = "Brno",
						Street = "Center",
						PostalCode = "666 00"
					}
				},
				PaymentId = 1,
				Payment = payment
			};

			mockOrderRepository.Setup(o => o.GetCart(It.IsAny<string>(), It.IsAny<int>())).Returns(cart);

			var controller = new CartController(mockUnitOfWork.Object);

			var result = controller.Order("user123", 1);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(cart, okResult.Value);
		}

		[Fact]
		public void ChooseShipping_ShouldReturnOk_WhenShippingMethodIsSelected()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockOrderRepository = new Mock<IOrderRepository>();
			var mockShippingRepository = new Mock<IShippingRepository>();
			mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
			mockUnitOfWork.Setup(u => u.ShippingRepository).Returns(mockShippingRepository.Object);

			var shipping = new Shipping();
			var cart = new Order();
			mockShippingRepository.Setup(s => s.Get(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(shipping);
			mockOrderRepository.Setup(o => o.GetCart(It.IsAny<string>(), It.IsAny<int>())).Returns(cart);

			var controller = new CartController(mockUnitOfWork.Object);
			var request = new ChooseShippingRequest { UserId = "user123", CartId = 1, ShippingId = 1 };

			var result = controller.ChooseShipping(request);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(cart, okResult.Value);
		}

		[Fact]
		public void ChoosePayment_ShouldReturnOk_WhenPaymentMethodIsSelected()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var mockOrderRepository = new Mock<IOrderRepository>();
			var mockPaymentMethodRepository = new Mock<IPaymentMethodRepository>();
			mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
			mockUnitOfWork.Setup(u => u.PaymentMethodRepository).Returns(mockPaymentMethodRepository.Object);

			var paymentMethod = new PaymentMethod
			{
				Name = "Card",
				Enabled = true,
				ShippingPaymentMethod = new List<ShippingPaymentMethod>
				{
					new ShippingPaymentMethod
					{
					ShippingId = 1,
					PaymentMethodId = 1,
					}
				}
			};

			var cart = new Order
			{
				Id = 1,
				CreatedDate = DateTime.UtcNow,
				OrderProducts = new List<OrderProduct>
				{
					new OrderProduct
					{
						ProductId = 1,
						Product = new Product
						{
							Name = "chair",
							Enabled = true,
							IsInStock = true,
							BuyLimit = 50,
							Cost = 10,
							CostWithTax = 15,
							CostBefore = 12,
							ImageUrl = String.Empty
						},
						OrderId = 1,
						Count = 3
					}
				}
			};

			mockPaymentMethodRepository.Setup(p => p.Get(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(paymentMethod);
			mockOrderRepository.Setup(o => o.GetCart(It.IsAny<string>(), It.IsAny<int>())).Returns(cart);

			var controller = new CartController(mockUnitOfWork.Object);
			var request = new ChoosePaymentRequest { UserId = "user123", CartId = 1, PaymentMethodId = 1 };

			var result = controller.ChoosePayment(request);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(cart, okResult.Value);

			Assert.NotNull(cart.Payment);
			Assert.Equal(cart.Payment.OrderId, cart.Id);
			Assert.Equal(cart.Payment.PaymentStatusId, 1);
			Assert.Equal(cart.Payment.Cost, 30);
			Assert.Equal(cart.Payment.CostWithTax, 45);
		}

	}
}