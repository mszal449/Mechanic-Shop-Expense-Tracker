import type { Metadata } from "next";
import localFont from "next/font/local";
import "./globals.css";
import { AuthProvider, Navbar } from "@/components";


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
      {/* <AuthProvider>  */}
        <body>
          <Navbar/>
          {children}
        </body>
      {/* </AuthProvider> */}
    </html>
  );
}
