import type { Metadata } from "next";
import "./globals.css";
import { Navbar } from "@/components";

const metadata: Metadata = {
  title: "Car Mechanic Expense Tracker",
  description: "A simple expense tracker for car mechanics",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Navbar />
        {children}
      </body>
    </html>
  );
}
