# das-digital-engagement

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-campaign-functions)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1309)

## Overview

A collection of Azure Functions which allow lead information to be imported into Marketo (SaaS Digital Engagement platform).  There are two main ways in which lead information is imported.  Firstly, via a manual CSV file upload, and secondly via an automated upload of employer leads taken from the service.

### CSV upload

A CSV file containing lead information can be uploaded to BLOB storage, which triggers an Azure function.  The function validates the CSV file and uploads it to Marketo.  A report is created (again, in BLOB storage) which details any issues in processing the import.  Such imports are adhoc, and the data in Marketo will only be updated again as and when an updated CSV file is uploaded for processing.

### Automated Employer Contact upload

An Azure function runs on a schedule (CRON), and on each execution all employer lead information is read from the Apprenticeship Service and imported into Marketo (whole dataset each time, not deltas).  As this is run on a schedule, on-service employer leads are always up to date in Marketo.  The import populates two items in Marketo, the out the box Lead object, and also a custom object to hold extended attributes.  A report is created (again, in BLOB storage) which details any issues in processing the import.

## Running locally

In order to run this repo locally, you will need follow the standard Microsoft instructions for running any Azure Functions locally - this is a prerequisite.  Additionally, you will require local Storage Account emulation, and  will need a local.settings.json with appropriate values.

This repo uses the standard Apprenticeship Service approach of configuration in table storage.  The configuration should be copied to your local Storage Account following the standard Apprenticeship Service model.

If debugging a CRON based Azure Function, it is recommended that you modify the CRON schedule to be something that will not interfere with your debugging session.  For example, you could use "0 0 1 * *", which is once per month, on the first of the month.  This then allows you to trigger the function manually, without fear of multiple execution of the function occurring during the debug session.

To trigger the function locally, you can POST a request to the following URL (standard Azure Function local debug approach).

http://localhost:7071/admin/functions/ImportDataMartData