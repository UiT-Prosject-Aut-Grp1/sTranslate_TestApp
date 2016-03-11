close all;
clear all;

% Read files and add to array
logging{1} = csvread('logCSharpNoCache.csv');
logging{2} = csvread('logCSharpWithCache.csv');
logging{3} = csvread('logFSharpDirectNoCache.csv');
logging{4} = csvread('logFSharpDirectWithCache.csv');
logging{5} = csvread('logFSharpNoCache.csv');
logging{6} = csvread('logFSharpCache.csv');

% Set names
name{1} = 'C# no cache';
name{2} = 'C# width cache';
name{3} = 'F# Direct no cache';
name{4} = 'F# Direct width cache';
name{5} = 'F# No Cache';
name{6} = 'F# cache';

% Set names
color{1} = [1 0 0];
color{2} = [0 1 0];
color{3} = [0 0 1];
color{4} = [1 1 0];
color{5} = [0 1 1];
color{6} = [1 0 1];

% Name graphics window
figure('name','Histogram');
for logNumber = 1:length(logging)
    
    % Convert to ms
    logging{logNumber} = logging{logNumber}*1000;
    
    % Remove first element
    first(logNumber) = logging{logNumber}(1);
    logging{logNumber}(1) = [];
    
    % Average
    average(logNumber) = mean(logging{logNumber});
    
    % Remover outliers
    i = 1;
    while i <= length(logging{logNumber})
        if logging{logNumber}(i) >  average(logNumber)*1.25
           logging{logNumber}(i) = [];
        elseif logging{logNumber}(i) <  average(logNumber)*0.75
           logging{logNumber}(i) = [];
        else
            i = i+1;
        end
    end
    
    % Make histogram
   % set(findobj(gca,'Type','patch'),'FaceColor',color{logNumber},'EdgeColor','w');
   % hist(logging{logNumber})
    
   subplot(3,2,logNumber)
   hist(logging{logNumber})
   title(name{logNumber})
   
end


% Average chart
hold off;
figure('name','Average');

y = [average(1) average(2); average(3) average(4); average(5)  average(6)];
bar(y)
set(gca,'XTickLabel',{'C#', 'F#', 'F# direct'})
legend('No cache', 'Width cache');

set(gca,'YScale','log')


% Average chart
hold off;
figure('name','First');

y = [first(1) first(2); first(3) first(4); first(5)  first(6)];
bar(y)
set(gca,'XTickLabel',{'C#', 'F#', 'F# direct'})
legend('No cache', 'Width cache');

set(gca,'YScale','log')