SELECT * FROM Users
WHERE RevokedOn IS NULL
  AND Birthday IS NOT NULL
  AND AGE(CURRENT_DATE, Birthday) > @Age * INTERVAL '1 year';