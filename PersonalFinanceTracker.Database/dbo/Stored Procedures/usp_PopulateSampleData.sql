
-- 9. Sample Data Procedure
-- ----------------------------------------

-- Populate sample data for testing
CREATE PROCEDURE usp_PopulateSampleData
    @UserID INT
AS
BEGIN
    -- Sample Accounts
    INSERT INTO Accounts (UserID, AccountName, AccountType, CurrentBalance, IsFinancialInstitution, IsMerchant, Category)
    VALUES 
        (@UserID, 'Checking Account', 'Checking', 2500.00, 0, 0, 'Personal'),
        (@UserID, 'Savings Account', 'Savings', 10000.00, 0, 0, 'Personal'),
        (@UserID, 'Credit Card', 'Credit Card', 1200.00, 0, 0, 'Personal'),
        (@UserID, 'Investment Account', 'Investment', 25000.00, 0, 0, 'Retirement'),
        (@UserID, 'Bank of America', 'Financial Institution', 0.00, 1, 0, 'Banking'),
        (@UserID, 'Amazon', 'Merchant', 0.00, 0, 1, 'Shopping'),
        (@UserID, 'Grocery Store', 'Merchant', 0.00, 0, 1, 'Groceries');
    
    -- Sample Budget Categories
    INSERT INTO BudgetCategories (UserID, CategoryName, CategoryType, MonthlyAllocation)
    VALUES
        (@UserID, 'Salary', 'Income', NULL),
        (@UserID, 'Bonus', 'Income', NULL),
        (@UserID, 'Housing', 'Expense', 1500.00),
        (@UserID, 'Groceries', 'Expense', 500.00),
        (@UserID, 'Utilities', 'Expense', 300.00),
        (@UserID, 'Entertainment', 'Expense', 200.00),
        (@UserID, 'Transportation', 'Expense', 400.00),
        (@UserID, 'Healthcare', 'Expense', 250.00),
        (@UserID, 'Dining Out', 'Expense', 300.00),
        (@UserID, 'Shopping', 'Expense', 200.00);
    
    -- Sample Transactions (for the last 3 months)
    DECLARE @StartDate DATE = DATEADD(MONTH, -3, GETDATE());
    DECLARE @EndDate DATE = GETDATE();
    DECLARE @CurrentDate DATE = @StartDate;
    DECLARE @CheckingID INT, @SavingsID INT, @CreditID INT, @InvestmentID INT;
    DECLARE @SalaryID INT, @BonusID INT, @HousingID INT, @GroceriesID INT, @UtilitiesID INT;
    DECLARE @EntertainmentID INT, @TransportationID INT, @HealthcareID INT, @DiningID INT, @ShoppingID INT;
    
    -- Get the IDs of created accounts
    SELECT @CheckingID = AccountID FROM Accounts WHERE UserID = @UserID AND AccountName = 'Checking Account';
    SELECT @SavingsID = AccountID FROM Accounts WHERE UserID = @UserID AND AccountName = 'Savings Account';
    SELECT @CreditID = AccountID FROM Accounts WHERE UserID = @UserID AND AccountName = 'Credit Card';
    SELECT @InvestmentID = AccountID FROM Accounts WHERE UserID = @UserID AND AccountName = 'Investment Account';
    
    -- Get the IDs of created categories
    SELECT @SalaryID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Salary';
    SELECT @BonusID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Bonus';
    SELECT @HousingID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Housing';
    SELECT @GroceriesID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Groceries';
    SELECT @UtilitiesID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Utilities';
    SELECT @EntertainmentID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Entertainment';
    SELECT @TransportationID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Transportation';
    SELECT @HealthcareID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Healthcare';
    SELECT @DiningID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Dining Out';
    SELECT @ShoppingID = CategoryID FROM BudgetCategories WHERE UserID = @UserID AND CategoryName = 'Shopping';
    
    -- Sample monthly salary (income) for each month
    WHILE @CurrentDate <= @EndDate
    BEGIN
        -- Monthly Salary (15th of each month)
        INSERT INTO Transactions (
            UserID, AccountID, CategoryID, TransactionDate, Amount, 
            MerchantName, Description, TransactionType, IsIncome
        )
        VALUES (
            @UserID, @CheckingID, @SalaryID, 
            DATEFROMPARTS(YEAR(@CurrentDate), MONTH(@CurrentDate), 15),
            5000.00, 'Employer', 'Monthly Salary', 'Recurring', 1
        );
        
        -- Monthly expenses
        -- Rent/Mortgage (1st of month)
        INSERT INTO Transactions (
            UserID, AccountID, CategoryID, TransactionDate, Amount, 
            MerchantName, Description, TransactionType, IsIncome
        )
        VALUES (
            @UserID, @CheckingID, @HousingID, 
            DATEFROMPARTS(YEAR(@CurrentDate), MONTH(@CurrentDate), 1),
            1500.00, 'Landlord', 'Monthly Rent', 'Recurring', 0
        );
        
        -- Utilities (5th of month)
        INSERT INTO Transactions (
            UserID, AccountID, CategoryID, TransactionDate, Amount, 
            MerchantName, Description, TransactionType, IsIncome
        )
        VALUES (
            @UserID, @CheckingID, @UtilitiesID, 
            DATEFROMPARTS(YEAR(@CurrentDate), MONTH(@CurrentDate), 5),
            250.00, 'Utility Company', 'Monthly Utilities', 'Recurring', 0
        );
        
        -- Move to next month
        SET @CurrentDate = DATEADD(MONTH, 1, @CurrentDate);
    END
    
    -- Sample one-time expenses (last month)
    DECLARE @LastMonth DATE = DATEADD(MONTH, -1, GETDATE());
    
    -- Groceries (multiple times)
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CreditID, @GroceriesID, 
        DATEADD(DAY, -5, GETDATE()),
        120.50, 'Grocery Store', 'Weekly Grocery Shopping', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @GroceriesID, 
        DATEADD(DAY, -12, GETDATE()),
        95.75, 'Grocery Store', 'Weekly Grocery Shopping', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @GroceriesID, 
        DATEADD(DAY, -19, GETDATE()),
        115.20, 'Grocery Store', 'Weekly Grocery Shopping', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @GroceriesID, 
        DATEADD(DAY, -26, GETDATE()),
        105.35, 'Grocery Store', 'Weekly Grocery Shopping', 'One-Time', 0
    );
    
    -- Dining out
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CreditID, @DiningID, 
        DATEADD(DAY, -3, GETDATE()),
        75.80, 'Nice Restaurant', 'Dinner with friends', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @DiningID, 
        DATEADD(DAY, -10, GETDATE()),
        45.25, 'Cafe', 'Lunch meeting', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @DiningID, 
        DATEADD(DAY, -17, GETDATE()),
        32.40, 'Fast Food', 'Quick lunch', 'One-Time', 0
    );
    
    -- Entertainment
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CreditID, @EntertainmentID, 
        DATEADD(DAY, -7, GETDATE()),
        65.00, 'Movie Theater', 'Movie night', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @EntertainmentID, 
        DATEADD(DAY, -14, GETDATE()),
        120.00, 'Concert Venue', 'Concert tickets', 'One-Time', 0
    );
    
    -- Transportation
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CreditID, @TransportationID, 
        DATEADD(DAY, -2, GETDATE()),
        45.00, 'Gas Station', 'Fuel', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @TransportationID, 
        DATEADD(DAY, -16, GETDATE()),
        48.50, 'Gas Station', 'Fuel', 'One-Time', 0
    );
    
    -- Healthcare
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CheckingID, @HealthcareID, 
        DATEADD(DAY, -20, GETDATE()),
        25.00, 'Pharmacy', 'Prescription', 'One-Time', 0
    ),
    (
        @UserID, @CheckingID, @HealthcareID, 
        DATEADD(DAY, -8, GETDATE()),
        150.00, 'Doctor Office', 'Co-pay for visit', 'One-Time', 0
    );
    
    -- Shopping
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CreditID, @ShoppingID, 
        DATEADD(DAY, -6, GETDATE()),
        120.50, 'Department Store', 'Clothing', 'One-Time', 0
    ),
    (
        @UserID, @CreditID, @ShoppingID, 
        DATEADD(DAY, -22, GETDATE()),
        85.75, 'Online Store', 'Electronics', 'One-Time', 0
    );
    
    -- Sample transfers to savings
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @CheckingID, NULL, 
        DATEADD(DAY, -25, GETDATE()),
        500.00, 'Transfer', 'Monthly savings transfer', 'Transfer', 0
    );
    
    INSERT INTO Transactions (
        UserID, AccountID, CategoryID, TransactionDate, Amount, 
        MerchantName, Description, TransactionType, IsIncome
    )
    VALUES 
    (
        @UserID, @SavingsID, NULL, 
        DATEADD(DAY, -25, GETDATE()),
        500.00, 'Transfer', 'Monthly savings transfer', 'Transfer', 1
    );
    
    -- Sample investments
    INSERT INTO Investments (
        UserID, AccountID, AssetName, AssetClass, Ticker, 
        Quantity, PurchasePrice, CurrentPrice, PurchaseDate
    )
    VALUES 
    (
        @UserID, @InvestmentID, 'S&P 500 Index Fund', 'Stocks', 'SPY', 
        10.0, 350.00, 375.00, DATEADD(MONTH, -6, GETDATE())
    ),
    (
        @UserID, @InvestmentID, 'Total Bond Market ETF', 'Bonds', 'BND', 
        20.0, 85.00, 82.50, DATEADD(MONTH, -6, GETDATE())
    ),
    (
        @UserID, @InvestmentID, 'Technology Sector ETF', 'Stocks', 'XLK', 
        5.0, 140.00, 155.00, DATEADD(MONTH, -3, GETDATE())
    ),
    (
        @UserID, @InvestmentID, 'REIT ETF', 'Real Estate', 'VNQ', 
        15.0, 95.00, 92.00, DATEADD(MONTH, -4, GETDATE())
    );
    
    -- Sample financial goals
    INSERT INTO FinancialGoals (
        UserID, GoalName, TargetAmount, CurrentAmount, StartDate, TargetDate,
        Category, Priority, Status, Description, MonthlyContribution
    )
    VALUES 
    (
        @UserID, 'Emergency Fund', 15000.00, 10000.00, 
        DATEADD(MONTH, -6, GETDATE()), 
        DATEADD(MONTH, 6, GETDATE()),
        'Emergency Fund', 'High', 'In Progress', 
        '6 months of expenses', 500.00
    ),
    (
        @UserID, 'Vacation Fund', 3000.00, 1200.00, 
        DATEADD(MONTH, -3, GETDATE()), 
        DATEADD(MONTH, 3, GETDATE()),
        'Vacation', 'Medium', 'In Progress', 
        'Summer vacation', 300.00
    ),
    (
        @UserID, 'New Car', 20000.00, 5000.00, 
        DATEADD(MONTH, -12, GETDATE()), 
        DATEADD(MONTH, 24, GETDATE()),
        'Other', 'Low', 'In Progress', 
        'Saving for a new car', 400.00
    );
    
    -- Sample payment methods
    INSERT INTO PaymentMethods (
        UserID, MethodName, MethodType, CurrentBalance
    )
    VALUES 
    (
        @UserID, 'Chase Credit Card', 'Credit Card', 1200.00
    ),
    (
        @UserID, 'Bank Debit Card', 'Debit Card', NULL
    ),
    (
        @UserID, 'PayPal', 'Digital Wallet', 150.00
    ),
    (
        @UserID, 'Cash', 'Cash', NULL
    );
END