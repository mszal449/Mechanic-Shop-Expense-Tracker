'use client'
import { apiUrl } from '@/const';
import { getAllJobs } from '@/services/jobService';
import { Job, Status } from '@/types/apiTypes';
import React, { useEffect, useState } from 'react';



const JobsTable = () => {
  const [jobs, setJobs] = useState<Job[] | null>(null);
  const [isLoading, setIsLoading ] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchJobs = async () => {
      try {
        setIsLoading(true);
        const jobsData = await getAllJobs();
        setJobs(jobsData);
      } catch (error: any) {
      } finally {
        setIsLoading(false);
      }
    };

    fetchJobs();
  }, []);
  

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full border-collapse border border-gray-300">

        {/* Table Head */}
        <thead>
          <tr>
            <th className="border border-gray-300 px-4 py-2">#</th>
            <th className="border border-gray-300 px-4 py-2">Name</th>
            <th className="border border-gray-300 px-4 py-2">Status</th>
            <th className="border border-gray-300 px-4 py-2">Mechanic</th>
            <th className="border border-gray-300 px-4 py-2">Created At</th>
          </tr>
        </thead>
        <tbody>

          {/* Loading spinner */}
          {isLoading && (
            <tr>
              <td colSpan={5} className="border border-gray-300 px-4 py-2 text-center">
                <div className="flex justify-center items-center space-x-2">
                  <span className="loader"></span>
                  <span>Loading...</span>
                </div>
              </td>
            </tr>
          )}

          {/* Error notification */}
          {!isLoading && error && (
            <tr>
              <td colSpan={5} className="border border-gray-300 px-4 py-2 text-center text-red-500">
                Error: {error}
              </td>
            </tr>
          )}

          {/* Result table content */}
          {!isLoading && jobs && jobs.map((job) => (
            <tr key={job.jobId}>
              <td className="border border-gray-300 px-4 py-2">{job.jobId}</td>
              <td className="border border-gray-300 px-4 py-2">{job.name}</td>
              <td className="border border-gray-300 px-4 py-2">{job.jobStatus}</td>
              <td className="border border-gray-300 px-4 py-2">{job.supervisor || '-'}</td>
              <td className="border border-gray-300 px-4 py-2">{job.createdAt.toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default JobsTable;
