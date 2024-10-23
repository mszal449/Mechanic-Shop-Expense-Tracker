'use client';

import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { authContext } from "@/components/auth/AuthProvider";
import { useAuth } from "@/components";

export default function Home() {

  return (
    <div className="pt-[200px] flex flex-col gap-4 items-center">
      <div className="flex flex-col items-center text-center">
        <div className="home--banner-title">
          Welcome to Expense Tracker for Car Mechanics
        </div>
        <div className="home--banner-subtitle">
          A business managment tool for tracking expenses and income for car
           mechanic shops.
        </div>
      </div>
      <a className="home--begin-button" href="/dashboard">Start!</a>
    </div>
  );
}
