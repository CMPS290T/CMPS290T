#! /bin/csh -f

cd ..
cd data/patient_data

foreach i (`ls`)
        echo "Messing with this " $i 
        python /home/super/hr4e/utilities/csv_magic/basics.py $i
        cd ..
end

echo "all done\!"

