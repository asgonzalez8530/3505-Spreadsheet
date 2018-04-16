// tests.cpp
#include "testThis.cpp"
#include "spreadsheet.h"
#include <gtest/gtest.h>



TEST(SquareRootTest, PositiveNos)
{
  ASSERT_EQ(6, squareRoot(36.0));
}



TEST(SpreadsheetTests, Constructor)
{
	cs3505::spreadsheet brandNewSheet("MyBrandNewFile");
}

TEST(SpreadsheetTests, SaveEmpty)
{
	cs3505::spreadsheet brandNewSheet("MyNewSaveFile");
	brandNewSheet.save();
}

TEST(SpreadsheetTests, SaveOneCell)
{

	std::cout << "Hello from test." << std::endl;
	cs3505::spreadsheet otherNewSheet("MyOtherNewSaveFile");
	std::cout << "Hello again." << std::endl;
	otherNewSheet.update("edit A1:36");
	otherNewSheet.save();
}

int main(int argc, char **argv)
{
  testing::InitGoogleTest(&argc, argv);
  return RUN_ALL_TESTS();
}
