import Link from "next/link";
import React from "react";

const Dashboard = () => {
  return (
    <div className="p-4 flex flex-col items-center">
      <h1 className="dashboard--title">Dashboard</h1>
      <div className="text-center p-4">
        <div className="flex flex-col gap-2">
          <Link href="/jobs" className="dashboard--element-container flex flex-col">
            <div className="dashboard--element-title">
              Jobs
            </div>
            <div className="dashboard--element-subtitle">
              Manage your jobs
            </div>
          </Link>
          <Link href="/statistics" className="dashboard--element-container">
          <div className="dashboard--element-title">
            Statistics
          </div>
          <div className="dashboard--element-subtitle" >
            See statistics about your business
          </div>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
