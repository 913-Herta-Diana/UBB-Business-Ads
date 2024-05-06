// <copyright file="FAQRepositoryTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Backend.Tests
{
    using Backend.Models;
    using Backend.Repositories;
    using Moq;
    using Xunit;

    public class FAQRepositoryTests
    {
        [Fact]
        public void GetFAQList_ReturnsFAQList()
        {
            // Arrange
            var repository = new FAQRepository();

            // Act
            var faqList = repository.GetFAQList();

            // Assert
            Assert.NotNull(faqList);
        }

        [Fact]
        public void AddFAQ_AddsFAQToList()
        {
            // Arrange
            var repository = new FAQRepository();
            var faq = new FAQ();

            // Act
            repository.AddFAQ(faq);

            // Assert
            Assert.Contains(faq, repository.GetFAQList());
        }

        [Fact]
        public void DeleteFAQ_RemovesFAQFromList()
        {
            // Arrange
            var repository = new FAQRepository();
            var faq = new FAQ();
            repository.AddFAQ(faq);

            // Act
            repository.DeleteFAQ(faq);

            // Assert
            Assert.DoesNotContain(faq, repository.GetFAQList());
        }
    }
}
