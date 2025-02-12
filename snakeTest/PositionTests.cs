using snakeLogic;

namespace snakeTest
{
    public class PositionTests
    {
        [Theory]
        [InlineData(1, 1, false)]
        [InlineData(10, 10, true)]

        public void CheckCollision_WithPosition_ExpectedResult(int x, int y, bool expectedResult)
        {
            //Arrange
            var position = new Position(10, 10);
            //Act
            var res = position.CheckCollision(new Position(x, y));
            //Assert
            Assert.Equal(expectedResult, res);
        }
    }
}