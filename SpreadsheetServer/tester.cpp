// tests.cpp
#include "testThis.cpp"
#include "spreadsheet.cpp"
#include <gtest/gtest.h>



TEST(SquareRootTest, PositiveNos)
{
  ASSERT_EQ(6, squareRoot(36.0));
}

TEST(SpreadsheetTests, Constructor)
{
	cs3505::spreadsheet("MyBrandNewFile");
}


int main(int argc, char **argv)
{
  testing::InitGoogleTest(&argc, argv);
  return RUN_ALL_TESTS();
}
