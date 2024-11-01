'use client'
import { JOB_STATUS } from '@/const';
import { getAllJobs } from '@/services/jobService';
import { Job } from '@/types/apiTypes';
import React, { useEffect, useState } from 'react';


const JobsTable = () => {
  const [jobs, setJobs] = useState<Job[] | null>(null);
  const [isLoading, setIsLoading ] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [totalCount, setTotalCount] = useState(0);
  const [selectedJobs, setSelectedJobs] = useState<Set<number>>(new Set());


  // calculate total page count
  const totalPages = Math.ceil(totalCount / pageSize);

  // Fetch job data
  useEffect(() => {
    const fetchJobs = async () => {
      try {
        setIsLoading(true);
        const jobsData = await getAllJobs({pageNumber: pageNumber, pageSize: pageSize});
        setJobs(jobsData.jobs);
        setTotalCount(jobsData.totalCount)
        if (error) setError(null);

      } catch (error: any) {
        setError(error)
      } finally {
        setIsLoading(false);
      }
    };

    fetchJobs();
  }, [pageNumber, pageSize]);


  // Handle selecting a job with checkbox
  const handleCheckboxChange = (jobId: number) => {
    setSelectedJobs((prevSelectedJobs) => {
      const newSelectedJobs = new Set(prevSelectedJobs);
      if (newSelectedJobs.has(jobId)) {
        newSelectedJobs.delete(jobId);
      } else {
        newSelectedJobs.add(jobId);
      }
      return newSelectedJobs;
    });
  };

  // Handle selecting all jobs with checkbox
  const handleSelectAll = () => {
    if (jobs) {
      if (selectedJobs.size === jobs.length) {
        setSelectedJobs(new Set());
      } else {
        setSelectedJobs(new Set(jobs.map((job) => job.jobId)));
      }
    }
  };

  // handle new page size
  const handlePageChange = (newPage: number) => {
    if (newPage > 0 && newPage <= totalPages) {
      setPageNumber(newPage);
    }
  };



  // Log selected jobs
  const logSelectedJobs = () => {
    console.log(jobs?.filter((job) => selectedJobs.has(job.jobId) ? job.jobId : null));
  };
  

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full border-collapse border border-gray-500">

        {/* Table Head */}
        <thead>
          <tr>
            <th className="border border-gray-500 px-4 py-2">
              <input type='checkbox'
                onChange={handleSelectAll}
              ></input>
            </th>
            <th className="border border-gray-500 px-4 py-2">#</th>
            <th className="border border-gray-500 px-4 py-2">Name</th>
            <th className="border border-gray-500 px-4 py-2">Status</th>
            <th className="border border-gray-500 px-4 py-2">Mechanic</th>
            <th className="border border-gray-500 px-4 py-2">Created At</th>
          </tr>
        </thead>
        <tbody>

          {/* Loading spinner */}
          {isLoading && (
            <tr>
              <td colSpan={6} className="border border-gray-500 px-4 py-2 text-center">
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
              <td colSpan={6} className="border border-gray-500 px-4 py-2 text-center text-red-500">
                An error occured while loading data
              </td>
            </tr>
          )}

          {/* Result table content */}
          {!isLoading && jobs && jobs.map((job) => (
            <tr key={job.jobId}>
              <td className="border border-gray-500 text-center">
                <input
                    type="checkbox"
                    checked={selectedJobs.has(job.jobId)}
                    onChange={() => handleCheckboxChange(job.jobId)}
                />
              </td>
              <td className="border border-gray-500 px-4 py-2">{job.jobId}</td>
              <td className="border border-gray-500 px-4 py-2">{job.name}</td>
              <td 
                className={`border border-gray-500 px-4 py-2 
                ${job.jobStatus === 0 ? 'text-orange-500' : 
                job.jobStatus === 1 ? 'text-blue-500' : 
                job. jobStatus === 2 ? 'text-green-500' : 
                job.jobStatus === 3 ? 'text-red-500' : ''}`}>
              {JOB_STATUS[String(job.jobStatus)].text}</td>
              <td className="border border-gray-500 px-4 py-2">{job.supervisor || '-'}</td>
              <td className="border border-gray-500 px-4 py-2">{job.createdAt.toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {/* operations on selected jobs */}
      {selectedJobs.size > 0 && 
        <button className='px-3 py-2 m-2 bg-red-500 rounded-md' onClick={logSelectedJobs}>Log</button>
      }
      {/* Pagination buttons */}
      {!isLoading && !error && 
      <div className="flex justify-center mt-4">
        <button
          className="px-3 py-2 mx-1 bg-gray-700 rounded-md disabled:bg-gray-900"
          onClick={() => handlePageChange(pageNumber - 1)}
          disabled={pageNumber === 1}
        >
          Previous
        </button>
        <span className="px-3 py-2 mx-1">{pageNumber}</span>
        <button
          className="px-3 py-2 mx-1 bg-gray-700 rounded-md disabled:bg-gray-900"
          onClick={() => handlePageChange(pageNumber + 1)}
          disabled={pageNumber >= totalPages}
        >
          Next
        </button>
      </div>
      }
    </div>
    
  );
};

export default JobsTable;
