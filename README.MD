# 📩 **Simple Messenger API**
> _Swagger-based ASP.NET messenger backend_

A **backend-only messenger** built for clean API interaction and easy testing via **Swagger UI**.  
Designed as a **portfolio project**, it focuses on robust architecture and extendable features — front-end integration can be added later.

---

## ✨ **Features**

### 🧍‍♂️ Users
- ✅ Register new users and store them in **PostgreSQL**
- 🔐 Authenticate via **JWT tokens** and **cookies**
- 📝 Edit user profile data
- 🔎 Retrieve all users or filter by parameters

### 💬 Chats
- 💌 Send private messages between users
- 🗂️ Retrieve full chat history with messages

### 👥 Group Chats
- 🆕 Create and manage **group chats**
- ➕ Add users to a group
- 🚪 Leave group chats
- 📨 View and send messages within a group

---

## 🧩 **Tech Stack**

| Layer | Technology |
|-------|-------------|
| **Backend** | ASP.NET Core |
| **Database** | PostgreSQL |
| **ORM** | Entity Framework Core |
| **Migrations** | Alembic |
| **Auth** | JWT (JSON Web Tokens) |
| **Testing** | Swagger UI |

---

## 🛠️ **Database Layer**

The project includes a **data access layer** with:
- Full PostgreSQL support
- Ready-to-use **migrations**
- Easy setup for both local and production environments


## 🔧 **Example API Calls**

### ➤ Register a user
`POST /api/users/register`
```json
{
  "login": "string",
  "password": "string",
  "NickName": "string"
}
```

### ➤ Send a message
`POST /api/chat/send`
```json
{
  "loginTo": "string",
  "text": "string"
}
```

### ➤ Create a group chat
`POST /api/group/create`
```json
{
  "groupName": "string",
  "loginsToAdd": "john_doe"
}
```

---

## 🤝 **Contributing & Collaboration**

Whether you’d like to:
- 🚀 Add new features
- 🧠 Improve architecture
- 💡 Build a front-end
- 🧩 Integrate external APIs

Feel free to **reach out or submit a pull request** — contributions are always welcome!

---

## 📫 **Contact**

📧 **Email:** [stanislavkapchenov@mail.ru](mailto:stanislavkapchenov@mail.ru)

---

### ⭐ **If you like the project — give it a star!**
Your support helps others discover it and motivates further development 🚀
