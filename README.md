# Inventory Management System (Frontend)

## 🧾 Overview

This is the **front-end** part of the the Inventory Management System (Graduation Project).  
It’s built using **Angular** and provides a user interface to manage inventories: products, categories, warehouses, shelves, accounts, and roles. It connects with a backend via RESTful APIs.

---

## 🛠 Tech Stack & Tools

- **Angular** (latest version used in project)
- **TypeScript**
- **HTML & CSS**
- **Angular Material** (for UI components such as tables, forms, etc.)
- **HTTPClientModule** (for API communication)
- **Reactive Forms**
- **Angular Router** (for navigation)
- **Services** to interact with APIs
- **Environments** for managing API base URLs

---

## 🎨 UI & Features

- Black and yellow theme
- Top navigation bar for main sections
- Responsive layout
- Tables for listing data
- Forms for create/edit operations
- Error handling & validation
- Loading indicators during API calls

---

## 📂 Project Structure (example)

```text
src/
 ├── app/
 │   ├── components/
 │   │   ├── products/
 │   │   ├── categories/
 │   │   ├── warehouses/
 │   │   ├── shelves/
 │   │   ├── accounts/
 │   │   └── roles/
 │   ├── navbar/
 │   ├── services/
 │   ├── models/
 │   ├── app-routing.module.ts
 │   └── app.component.ts
 ├── assets/
 ├── environments/
 ├── styles.css
 └── main.ts
```
## 🧩 Notes

- Ensure environment files (`environment.ts`, `environment.prod.ts`) point to the correct backend API
- Follow clean code practices for consistency

---

## ⚙️ Installation & Running Locally

1. Clone the repo:

   ```bash
   git clone https://github.com/M0hamedJR/graduation-project.git
   cd graduation-project
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Run the dev server:

   ```bash
   ng serve
   ```

   Then open your browser at: `http://localhost:4200`

4. Build for production:
   ```bash
   ng build --configuration production
   ```

---
