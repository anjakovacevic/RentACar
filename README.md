# Rent-a-Car WPF Application

## Project Overview

This project involves the development of a WPF (Windows Presentation Foundation) application for managing data within the Rent-a-Car domain. It was a part of final year project for _Projektovanje Softvera u Sistemima Upravljanja_ The application allows users to perform CRUD (Create, Read, Update, Delete) operations on two related tables in a database: `Cars` and `Locations`. These tables have a one-to-many (1:N) relationship, where a location can have multiple cars.

## Specifications

### Data Model

- **Classes Defined**: Detailed classes for `Cars` and `Locations` were created.
- **Entity Framework**: Used to generate the database, consisting of two tables (`Cars` and `Locations`) with a 1:N relationship.

### User Interface

- **Main Window**: 
  - Displays key data in a tabular view.
  - Contains buttons for performing all CRUD operations.

- **Create Window**:
  - Form to add a new object to the corresponding database table.

- **Read/Details Window**:
  - Displays detailed information about a selected object.

- **Update Window**:
  - Uses the create form to edit the selected object.

- **Delete Confirmation**:
  - Opens a window to confirm the deletion action, ensuring user intent.

### Data Binding and Real-Time Updates

- All data is adequately bound to ensure the tabular view refreshes in real-time when changes occur.

### Validation and Error Handling

- Implemented necessary validations and provided appropriate feedback for invalid inputs.
- Prevented application crashes when attempting to save invalid data (e.g., adding a duplicate primary key or a non-existent foreign key).
