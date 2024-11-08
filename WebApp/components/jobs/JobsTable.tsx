'use client'
import { JOB_STATUS } from '@/const';
import { BulkDeleteJobsAsync, getAllJobsAsync } from '@/services/jobService';
import { Job } from '@/types/apiTypes';
import React, { useEffect, useState } from 'react';
import Pagination from './Pagination';
import { useRouter } from 'next/navigation';

interface JobsTableProps {
  filters: any;
}

const JobsTable = ({ filters } : JobsTableProps) => {
  const [jobs, setJobs] = useState<Job[] | null>(null);
  const [isLoading, setIsLoading ] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [totalCount, setTotalCount] = useState(0);
  const [selectedJobs, setSelectedJobs] = useState<Set<number>>(new Set());
  const [isSelectedAllChecked, setIsSelectAllChecked] = useState<boolean>(false);
  const router = useRouter();

  // calculate total page count
  const totalPages = Math.ceil(totalCount / pageSize);

  const fetchJobs = async () => {
    setIsLoading(true);
    
    // clear job selection after page refresh/page number change
    setIsSelectAllChecked(false);
    setSelectedJobs(new Set());
    try {
      // fetch data and update states
      const jobsData = await getAllJobsAsync({
        pageNumber: pageNumber, 
        pageSize: pageSize, 
        ...filters});

      setJobs(jobsData.jobs);
      setTotalCount(jobsData.totalCount);
      if (error) setError(null);
    } catch (error: any) {
      setError(error)
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchJobs();
  }, [pageNumber, pageSize, filters]);

  // checkbox change for single record
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

  // checkbox change for all records
  const handleSelectAll = () => {
    if (jobs) {
      if (selectedJobs.size === jobs.length) {
        setSelectedJobs(new Set());
        setIsSelectAllChecked(false);
      } else {
        setSelectedJobs(new Set(jobs.map((job) => job.jobId)));
        setIsSelectAllChecked(true);
      }
    }
  };

  const handleBulkDelete = async () => {
    const selectedJobsArray: number[] = Array.from(selectedJobs)
    await BulkDeleteJobsAsync(selectedJobsArray);
    setSelectedJobs(new Set<number>());
    fetchJobs();
  };
  
  const formatDate = (date: Date) => {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'numeric',
      day: 'numeric',
      hour: 'numeric',
      minute: 'numeric',
      hour12: false,
    }).format(date);
  };

  const handleRowClick = (jobId: number) => {
    router.push(`/jobs/${jobId}`);
  };

  return (
    <div className="px-10">
      <div className='flex gap-2'>
        {/* Bulk operations buttons */}
        <div className="mb-2" 
          style={
            { visibility: !isLoading && !error ? 'visible' : 'hidden' }
          }>
          <button
            className="p-2 text-white bg-red-400 enabled:hover:bg-red-500 transition ease-in duration-150  rounded-md disabled:opacity-50"
            onClick={handleBulkDelete}
            disabled={selectedJobs.size === 0}
            >
            Delete Selected
          </button>
        </div>
      </div>
      <table className="min-w-full rounded-t-md border-collapse pb-10">
        {/* Table Head */}
        <thead>
          <tr>
            <th className="table--head">
              <input type='checkbox' className='cursor-pointer'
                onChange={handleSelectAll}
                checked={isSelectedAllChecked}
              ></input>
            </th>
            <th className="table--head">#</th>
            <th className="table--head">Name</th>
            <th className="table--head">Status</th>
            <th className="table--head">Supervisor</th>
            <th className="table--head">Created At</th>
          </tr>
        </thead>
        <tbody>

          {/* Loading spinner */}
          {isLoading && (
            <tr>
              <td colSpan={6} className="border border-gray-700 px-4 py-2 text-center">
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
              <td colSpan={6} className="border border-gray-700 px-4 py-2 text-center text-red-500">
                An error occured while loading data
              </td>
            </tr>
          )}

          {/* Result table content */}
          {!isLoading && jobs && jobs.map((job) => (
            <tr key={job.jobId}
            className={`
              ${selectedJobs.has(job.jobId) ? "bg-slate-900" : ""}
              hover:bg-slate-900 transition ease-in duration-50`}>
              <td className="table--data text-center">
                <input
                    type="checkbox"
                    className='cursor-pointer'
                    checked={selectedJobs.has(job.jobId)}
                    onChange={(e) => { 
                      e.stopPropagation();
                      handleCheckboxChange(job.jobId)}
                    }
                />
              </td>
              <td className="table--data text-center w-20">{job.jobId}</td>
              <td 
                className="table--data cursor-pointer"
                onClick={() => handleRowClick(job.jobId)}
              >
                {job.name}
              </td>
              <td 
                className={`table--data 
                ${job.jobStatus === 0 ? 'text-orange-500' : 
                job.jobStatus === 1 ? 'text-blue-500' : 
                job. jobStatus === 2 ? 'text-green-500' : 
                job.jobStatus === 3 ? 'text-red-500' : ''}`}>
              {JOB_STATUS[String(job.jobStatus)].text}</td>
              <td className="table--data">{job.supervisor || '-'}</td>
              <td className="table--data">{formatDate(new Date(job.createdAt))}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {/* Pagination buttons */}
      {!isLoading && !error && 
        <Pagination
          pageNumber={pageNumber}
          totalPages={totalPages}
          setPageNumber={setPageNumber}
        />
      }
    </div>
    
  );
};

export default JobsTable;
