# 🏥 Doctor Appointment & Telemedicine Platform (Backend)

A scalable **Doctor Appointment and Telemedicine backend system** designed to connect patients with doctors through secure booking, real-time communication, and flexible payment workflows.

This project focuses on **clean architecture, transactional business logic, and real-world healthcare workflows**, inspired by enterprise-grade systems such as Zocdoc.

> ⚠️ **Note:** Source code for the original enterprise system is confidential.
> This repository represents a **functional mirror** showcasing the same architecture, patterns, and workflows with dummy data.

---

## 🚀 Key Features

### 👤 Patient

* Secure registration and login using **OTP / Google Authentication**
* Location-based doctor search (Google Maps integration)
* Doctor booking with conflict prevention
* Multiple payment methods: **PayPal, Stripe, Cash**
* Booking management (cancel, reschedule, rebook)
* Reviews & ratings
* Real-time chat with doctors
* Notifications for bookings and updates

---

### 🧑‍⚕️ Doctor

* Admin-created accounts with forced password reset
* Availability and schedule management
* Booking approval, cancellation, and rescheduling
* Real-time chat with patients
* Earnings and booking reports
* Notifications for bookings, reviews, and messages

---

### 🛠️ Admin / Helpers

* Secure dashboard with **Role-Based Access Control (RBAC)**
* Doctor account creation and verification
* User and booking management
* Payment monitoring and dispute handling
* System statistics and reports
* Content management (FAQs, policies)
* Helper roles with scoped permissions

---

## 🧱 System Architecture

The backend follows **Clean Architecture** with clear separation of concerns:

```
├── Domain
│   ├── Entities
│   ├── Value Objects
│   └── Enums
│
├── Application
│   ├── CQRS (Commands / Queries)
│   ├── DTOs
│   ├── Validators
│   └── Interfaces
│
├── Infrastructure
│   ├── Persistence (ORM)
│   ├── Authentication & OTP Services
│   ├── Payment Gateways
│   └── Notification Services
│
└── API
    ├── Controllers
    ├── Middleware
    └── Real-time Communication (SignalR / WebSockets)
```

---

## 🔁 Core Business Flows

### 🔐 Authentication

* OTP-based registration and login
* Google authentication
* Role-based access control (Patient / Doctor / Admin)

### 📅 Booking Workflow

1. Patient selects doctor and available time slot
2. System validates availability and booking conflicts
3. Payment is processed
4. Booking is confirmed atomically
5. Notifications are sent to patient and doctor

### 💬 Real-Time Chat

* WebSocket-based messaging
* Message read status & unread counters
* Secure and reliable delivery

---

## 🧪 Non-Functional Requirements

* API response time < **2 seconds**
* Supports **100+ concurrent users**
* Secure authentication and encrypted sensitive data
* Transaction-safe operations
* Scalable and cloud-ready design
* Modular and maintainable codebase

---

## 🛡️ Security

* JWT-based authentication
* OTP verification
* Role-Based Authorization (RBAC)
* HTTPS enforced
* Secure payment handling

---

## 🛠️ Tech Stack (Representative)

* **Backend:** ASP.NET Core (Clean Architecture)
* **Database:** SQL Server 
* **Authentication:** JWT, OTP, Google Auth
* **Payments:** PayPal, Stripe
* **Real-Time:** SignalR 
* **Maps:** Google Maps API
* **Notifications:** Email / Push Notifications

---

## 📌 Why This Project Matters

This project demonstrates:

* Real-world healthcare workflows
* Strong backend architecture and patterns
* Secure authentication and payment integrations
* Transaction handling and data consistency
* Enterprise-ready system design

It reflects **how production systems are designed and built**, not just basic CRUD APIs.


## 📬 Contact

**Abdelrahman Tolba Sedeek**
Back-End Developer (.NET / ASP.NET Core)

* 📧 Email: [abdurahmansedeek@gmail.com](mailto:abdurahmansedeek@gmail.com)
* 🔗 GitHub: [https://github.com/abdelrahman-sedeek](https://github.com/abdelrahman-sedeek)
* 🔗 LinkedIn: [https://linkedin.com/in/abdelrahman-tolba-sedeek-1188391b0](https://linkedin.com/in/abdelrahman-tolba-sedeek-1188391b0)
