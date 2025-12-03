add_executable(day3)
target_sources(day3
    PRIVATE
    day3.cpp)
target_link_libraries(day3 read_file)
target_include_directories(day3 PRIVATE ../utils)
