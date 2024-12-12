This is a [Next.js](https://nextjs.org/) project bootstrapped with [`create-next-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

# C# To-Do List Manager

**Techstack**: C#, File I/O, JSON, Plain Text

- Created a command-line application that provides task management capabilities, allowing users to add, view, update, delete, and search tasks using unique identifiers, with each task featuring a title, description, due date, priority, and status.
- Developed robust task ID generation and error checking to ensure ID uniqueness and prevent conflicts.
- Incorporated file-based persistence to store task data between sessions and retrieve it upon startup.
- Designed with various filters and search options, enabling users to efficiently locate and manage tasks.

**Features**:
- **Add a Task**: Allows users to enter details such as ID, title, description, due date, priority, and status.
- **View Tasks**: Displays tasks with filtering options based on ID, status, priority, or due date.
- **Delete a Task**: Facilitates task deletion by unique identifier.
- **Update a Task**: Enables task updates for status and due date modifications.
- **Search Tasks**: Supports keyword search for task titles or descriptions.
- **Persistence**: Saves and loads tasks in a file format for consistent data retention between sessions.


## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.js`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/basic-features/font-optimization) to automatically optimize and load Inter, a custom Google Font.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js/) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/deployment) for more details.
