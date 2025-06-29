<div align="center">
   <p>
   <img src="social-campus-client/public/android-chrome-512x512.png" alt="Social Campus Logo"/>
   </p>
</div>

# Social Campus

**Social Campus** is a social networking platform specifically designed for students, providing a safe environment for communication, content sharing, and event planning. The platform combines traditional social media features with additional tools for organizing student life, ranging from photo and video sharing to creating events and interest-based communities.

## Table of Contents

- [Project Overview](#project-overview)
- [Goals and Strategy](#goals-and-strategy)
- [Target Audience](#target-audience)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Project Overview

Social Campus aims to create a secure space for students to interact, share information and content, plan joint events, and develop their projects. The platform offers a personalized experience for each user, allowing them to customize their news feed according to their interests while supporting group projects and educational initiatives.

## Goals and Strategy

The primary goal of the platform is to provide a safe space for students where they can:

- Interact and share content
- Plan and organize events
- Develop and grow their projects

The main strategy is to position the platform as a key hub for students, where they can actively engage, exchange experiences, and organize student events.

## Target Audience

The target audience includes students, young people, and university staff aged 16 to 50 who actively participate in student life and are interested in learning, communication, sharing experiences, and planning events. The website will be presented in English.

## Features

- **User Profiles**: Personalize your profile to connect with others.
- **News Feed**: Tailored news feed based on your interests and interactions.
- **Content Sharing**: Share photos, videos, and updates with your peers.
- **Likes and Comments**: Engage with content by liking posts and leaving comments.

## Technologies Used

- **Frontend**: React.js
- **Backend**: C# ASP.NET Core
- **Database**: PostgreSQL with Entity Framework Core
- **Object Storage**: MinIO
- **Email Testing**: MailHog

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/PalamarchukOleksii/Social-Campus.git
   ```

2. Navigate to the project directory:

   ```bash
   cd Social-Campus
   ```

3. Start required services (PostgreSQL, MinIO, and MailHog) using Docker Compose:

   ```bash
   docker-compose up -d
   ```

   > **Note:** All Docker Compose services are intended **only for local development and testing** purposes.
   > MailHog is included for capturing and testing emails locally without sending them.

4. Install dependencies for the frontend:

   ```bash
   cd social-campus-client
   npm install
   ```

5. Install dependencies for the backend:

   ```bash
   cd ../social-campus-server
   dotnet restore
   ```

6. Start the backend server:

   ```bash
   cd Presentation
   dotnet watch run
   ```

7. Start the frontend server:

   ```bash
   cd ../social-campus-client
   npm start
   ```

## Usage

After starting both the frontend and backend servers, navigate to:

```
http://localhost:3000
```

to access the Social Campus platform.

### Accessing MailHog

To view the emails sent locally during development (e.g., verification emails), open MailHog’s web UI at:

```
http://localhost:8025
```

This interface allows you to inspect, read, and test emails without sending them outside your local environment.

## Contributing

Contributions are welcome! If you would like to contribute to the project, please fork the repository and create a pull request. Ensure that you follow the project's coding standards and guidelines.

## License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

## Contact

For inquiries or feedback, please reach out to [oleksiypalamarchuck@gmail.com](mailto:oleksiypalamarchuck@gmail.com?subject=Social%20Campus%20Inquiry&body=Hello%20Oleksii,)
