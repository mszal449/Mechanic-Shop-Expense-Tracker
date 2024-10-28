import Link from "next/link";
import React from "react";

const Dashboard = () => {
  return (
    <div className="p-4 flex flex-col items-center">
      <h1 className="dashboard--title">Dashboard</h1>
      <div className="text-center">
        <div className="flex flex-col gap-2">
          <Link href="/jobs" className="text-blue-500 hover:underline flex">
            Jobs
          </Link>
          <Link href="/statistics" className="text-blue-500 hover:underline">
            Statistics
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
