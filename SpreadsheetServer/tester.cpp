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

	//std::cout << "Hello for commit from test." << std::endl;
	//std::cout << "Hello from test." << std::endl;
	cs3505::spreadsheet otherNewSheet("MyOtherNewSaveFile");
	//std::cout << "Hello again." << std::endl;
	otherNewSheet.update("edit A1:36");
	otherNewSheet.save();
}

TEST(SpreadsheetTests, RevertOneCell)
{

	//std::cout << "Hello for commit from test." << std::endl;
	//std::cout << "Hello from test." << std::endl;
	cs3505::spreadsheet otherNewSheet("MyNewSaveFile");
	//std::cout << "Hello again." << std::endl;
	otherNewSheet.update("edit A1:36");
	otherNewSheet.update("edit A1:I love dogs!");
	otherNewSheet.update("revert A1");
	otherNewSheet.save();
}

TEST(SpreadsheetTests, UndoOneCell)
{

	//std::cout << "Hello for commit from test." << std::endl;
	//std::cout << "Hello from test." << std::endl;
	cs3505::spreadsheet otherNewSheet("UndoSaveFile");
	//std::cout << "Hello again." << std::endl;
	
	otherNewSheet.update("edit A1:36");
	std::string change1 = otherNewSheet.update("edit A1:I love dogs!");
	ASSERT_EQ("change A1:I love dogs!", change1);
	
	std::string change = otherNewSheet.update("revert A1");
	ASSERT_EQ("change A1:36", change);

	std::string change2 = otherNewSheet.update("undo ");
	ASSERT_EQ("change A1:I love dogs!", change2);
	otherNewSheet.save();
}

int main(int argc, char **argv)
{
  testing::InitGoogleTest(&argc, argv);
  return RUN_ALL_TESTS();
}
