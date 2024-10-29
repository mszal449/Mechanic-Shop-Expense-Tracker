'use client'
import { getAllJobs } from '@/services/jobService';
import { Job } from '@/types/apiTypes';
import React, { useEffect, useState } from 'react';


const JobsTable = () => {
  const [jobs, setJobs] = useState<Job[] | null>(null);
  const [isLoading, setIsLoading ] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);

  useEffect(() => {
    const fetchJobs = async () => {
      try {
        setIsLoading(true);
        const jobsData = await getAllJobs(pageNumber, pageSize);
        setJobs(jobsData);
        if (error) setError(null);

      } catch (error: any) {
        setError(error)
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
            <th className="border border-gray-300 px-4 py-2">
              <input type='checkbox'></input>
            </th>
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
                {error.toString()}
              </td>
            </tr>
          )}

          {/* Result table content */}
          {!isLoading && jobs && jobs.map((job) => (
            <tr key={job.jobId}>
              <td className="border border-gray-300 text-center"><input type='checkbox'></input></td>
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
