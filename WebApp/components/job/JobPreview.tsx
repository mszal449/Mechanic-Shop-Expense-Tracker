'use client';
import { JOB_STATUS } from '@/const';
import { getJobAsync } from '@/services/jobService';
import React, { useEffect, useState } from 'react';


const JobPreview = ({ jobId }: { jobId: number }) => {
  const [job, setJob] = useState<Job | null>(null);
  const [error, setError] = useState<string | null>(null);

  // Fetch job data
  useEffect(() => {
    const fetchJob = async () => {
      try {
        const jobResult = await getJobAsync(jobId);
        setJob(jobResult);
        setError(null);
      } catch (error: any) {
        setError(error.message || 'An error occurred while fetching the job.');
      }
    };

    fetchJob();
  }, [jobId]);

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    }).format(date);
  };

  return (
    <div className="grid grid-cols-12 shadow-md w-full">
      {job ? (
        <div className='p-2 col-start-4 col-end-10 border border-gray-800 rounded-md'>
          {/* Title Row */}
          <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-2">
            <h1 className="text-3xl font-light text-gray-300">{job.name}</h1>
          </div>

          {/* Info and Actions Row */}
          <div className="flex flex-col md:flex-row md:items-center md:justify-between">
            {/* Job Details */}
            <div className="flex flex-wrap gap-4">
              <div className="flex items-center">
                <span className="font-medium text-gray-500">Status:</span>
                <span className={`ml-2 ${JOB_STATUS[String(job.jobStatus)].color}`}>
                  {JOB_STATUS[String(job.jobStatus)].text}
                </span>
              </div>
              <div className="flex items-center">
                <span className="font-medium text-gray-500">Supervisor:</span>
                <span className="ml-2 text-gray-500">{job.supervisor || 'N/A'}</span>
              </div>
              <div className="flex items-center">
                <span className="font-medium text-gray-500">Created At:</span>
                <span className="ml-2 text-gray-500">{formatDate(job.createdAt.toString())}</span>
              </div>
            </div>
          </div>
        </div>
      ) : error ? (
        <div className="text-red-500 text-center">There was en error while loading the information...</div>
      ) : (
        <div className="flex justify-center items-center">
          <span className="loader mr-2"></span>
          <span>Loading...</span>
        </div>
      )}
    </div>
  );
};

export default JobPreview;