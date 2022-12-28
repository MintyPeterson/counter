-- <copyright file="AddEntryEstimateIndicator.sql" company="Tom Cook">
-- Copyright (c) Tom Cook. All rights reserved.
-- </copyright>

--
-- DESCRIPTION
--   This script adds a column to the entries table to
--   indicate if the entry is an estimate or not.
--

ALTER TABLE Entries ADD IsEstimate bit NOT NULL DEFAULT(0)