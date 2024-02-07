namespace CardRules.RulesVerification.TestSuite;

public class UnitTest
{
    private ValidationService service;
    private readonly ITestOutputHelper output;

    public UnitTest(ITestOutputHelper testOutput)
    {
        var mockInterface = new Mock<ILoggerManager>();
        output = testOutput;
        service = new ValidationService(mockInterface.Object);
    }

    [Fact]
    public async Task CardCheck_ShouldAllowValidCardApplicant()
    {
        //Arrange
        bool expectedResult = true;
        CardApplicationRequest card = new()
        {
            isNewApplicant = true,
            AccountNo = "23252112700000701",
            BinNo = "95030599"
        };
        
        //Act
        var result = await service.cardcheck(card,string.Empty);
        var data = result.Item2.Data;

        //Assert
        Assert.Equal(expectedResult,data.ResultStatus);

        //Output
       output.WriteLine($"RuleResult: {data.ResultStatus}\n" +
           $"Failed Workflow Name : {data.FailedWorkflowName}\n" +
           $"Error Message : {data.ErrorMessage}");
    }
   
    [Fact]
    public async Task CardRuleCheck_ShouldNotAllowInvalidAccClass()
    {
        //Arrange
        bool expectedResult = false;
        CardApplicationRequest card = new()
        {
            isNewApplicant = true,
            ExistingBinNoList = new List<BinNOs> { new BinNOs { BinNo = "95030599" } },
            AccountNo = "23233312700000701",
            BinNo = "95030599"
        };
        
        //Act
        var result = await service.cardcheck(card, string.Empty);
        var data = result.Item2.Data;

        //Assert
        Assert.Equal(expectedResult, data.ResultStatus);

        //Output
       output.WriteLine($"RuleResult: {data.ResultStatus}\n" +
           $"Failed Workflow Name : {data.FailedWorkflowName}\n" +
           $"Error Message : {data.ErrorMessage}");
    }
    
    [Fact]
    public async Task CardCheck_ShouldNotAllowExistingBinNo()
    {
        //Arrange
        bool expectedResult = false;
        CardApplicationRequest card = new()
        {
            isNewApplicant = true,
            ExistingBinNoList = new List<BinNOs> { new BinNOs { BinNo = "95030599" } },
            AccountNo = "23252112700000701",
            BinNo = "95030599"
        };

        //Act
        var result = await service.cardcheck(card, string.Empty);
        var data = result.Item2.Data;

        //Assert
        Assert.Equal(expectedResult, data.ResultStatus);
        
        //Output
       output.WriteLine($"RuleResult: {data.ResultStatus}\n" +
           $"Failed Workflow Name : {data.FailedWorkflowName}\n" +
           $"Error Message : {data.ErrorMessage}");

    }

    [Fact]
    public async Task CardCheck_ShouldNotAllowInvalidAccClassBaseOnBinNo()
    {
        //Arrange
        bool expectedResult = false;
        CardApplicationRequest card = new()
        {
            isNewApplicant = true,
            ExistingBinNoList = new List<BinNOs> { new BinNOs { BinNo = "95030510" } },
            AccountNo = "23230112700000701",
            BinNo = "95030599"
        };

        //Act
        var result = await service.cardcheck(card, string.Empty);
        var data = result.Item2.Data;

        //Assert
        Assert.Equal(expectedResult, data.ResultStatus);

        //Output
        output.WriteLine($"RuleResult: {data.ResultStatus}\n" +
            $"Failed Workflow Name : {data.FailedWorkflowName}\n" +
            $"Error Message : {data.ErrorMessage}");
    }
}